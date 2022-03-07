using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Common.Runtime;
using Common.Nodes;
using Common.Editor.Nodes;

namespace Common.Editor
{
    public class NodeGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeGraphView, UxmlTraits> { }

        private NodeGraph _nodeGraph;
        public Action<NodeView> NodeSelected;

        public NodeGraphView()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            GridBackground gridBackground = new GridBackground();
            
            Insert(0, gridBackground);
            gridBackground.StretchToParentSize();

            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Common/Scripts/Editor/NodeGraphEditorWindow.uss");
            styleSheets.Add(style);
        }

        internal void PopulateView(NodeGraph nodeGraph)
        {
            _nodeGraph = nodeGraph;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += OnGraphViewChanged;

            if (_nodeGraph.RootNode == null)
            {
                _nodeGraph.RootNode = ScriptableObject.CreateInstance<ResultNode>();
                _nodeGraph.RootNode.name = nodeGraph.RootNode.GetType().Name;
                _nodeGraph.RootNode.Guid = GUID.Generate().ToString();
                _nodeGraph.AddNode(_nodeGraph.RootNode);
            }

            _nodeGraph.Nodes.ForEach(n => CreateAndAddNodeView(n));

            _nodeGraph.Nodes.ForEach(n =>
            {
                if (n is IntermediateNode intermediateNode)
                {
                    NodeView parentView = FindNodeView(n);
                    for (int i = 0; i < intermediateNode.children.Count; i++)
                    {
                        NodeView childView = FindNodeView(intermediateNode.children[i]);
                        Edge edge = parentView.Inputs[i].ConnectTo(childView.Output);
                        AddElement(edge);
                    }
                }
                else if (n is ResultNode rootNode)
                {
                    if (rootNode.child != null)
                    {
                        NodeView parentView = FindNodeView(n);
                        NodeView childView = FindNodeView(rootNode.child);
                        Edge edge = parentView.Inputs[0].ConnectTo(childView.Output);
                        AddElement(edge);
                    }
                }
            });
        }

        private NodeView FindNodeView(FunctionNode node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(element =>
                {
                    if (element is NodeView nodeView)
                    {
                        _nodeGraph.DeleteNode(nodeView.Node);
                    }
                    else if (element is Edge edge)
                    {
                        NodeView parentView = edge.input.node as NodeView;
                        NodeView childView = edge.output.node as NodeView;
                        _nodeGraph.RemoveChild(parentView.Node, childView.Node, edge.input.portName);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.input.node as NodeView;
                    NodeView childView = edge.output.node as NodeView;
                    _nodeGraph.AddChild(parentView.Node, childView.Node, edge.input.portName);
                });
            }

            return graphViewChange;
        }

        private void CreateAndAddNodeView(FunctionNode node)
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(NodeView).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(NodeType), false) is NodeType[] attrs && attrs.Length > 0)
                {
                    if (attrs[0].Type == node.GetType())
                    {
                        NodeView nodeView = (NodeView)Activator.CreateInstance(type);
                        nodeView.Node = node;
                        nodeView.viewDataKey = node.Guid;
                        nodeView.style.left = node.Position.x;
                        nodeView.style.top = node.Position.y;
                        AddNodeView(nodeView);
                    }
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()
                .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }


        internal void AddNodeView(NodeView nodeView)
        {
            nodeView.NodeSelected = NodeSelected;
            AddElement(nodeView);
        }
    }
}
