using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using Common.Runtime;
using UnityEditor.Experimental.GraphView;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Editor.Nodes;

namespace Common.Editor
{
    public class NodeGraphEditorWindow : EditorWindow, ISearchWindowProvider
    {
        private NodeGraph _nodeGraph;
        private NodeGraphView _nodeGraphView;
        private VisualElement _leftPanel;
        private Texture2D m_Icon;
        private UnityEditor.Editor m_Editor;

        public static void ShowWindow(NodeGraph nodeGraph)
        {
            NodeGraphEditorWindow window = GetWindow<NodeGraphEditorWindow>();
            
            window.SelectNodeGraph(nodeGraph);
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("NodeGraph");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is NodeGraph nodeGraph)
            {
                ShowWindow(nodeGraph);
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Common/Scripts/Editor/NodeGraphEditorWindow.uxml");
            visualTree.CloneTree(root);
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Common/Scripts/Editor/NodeGraphEditorWindow.uss");
            root.styleSheets.Add(styleSheet);

            _leftPanel = root.Q("LeftPanel");
            _nodeGraphView = root.Q<NodeGraphView>();
            _nodeGraphView.nodeCreationRequest += OnRequestNodeCreation;
            _nodeGraphView.NodeSelected = OnNodeSelected;
        }

        private void OnNodeSelected(NodeView nodeView)
        {
            _leftPanel.Clear();
            DestroyImmediate(m_Editor);
            m_Editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (m_Editor && m_Editor.target)
                {
                    m_Editor.OnInspectorGUI();
                }
            });
            _leftPanel.Add(container);
        }
        private void OnRequestNodeCreation(NodeCreationContext context)
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), this);
        }

        private void OnEnable()
        {
            // Transparent icon to trick search window into indenting items
            m_Icon = new Texture2D(1, 1);
            m_Icon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            m_Icon.Apply();
        }
        private void OnSelectionChange()
        {
            if (Selection.activeObject is NodeGraph nodeGraph)
            {
                SelectNodeGraph(nodeGraph);
            }
        }

        private void SelectNodeGraph(NodeGraph nodeGraph)
        {
            _nodeGraph = nodeGraph;
            _nodeGraphView.PopulateView(_nodeGraph);
        }

        internal struct NodeEntry
        {
            public string[] title;
            public NodeView nodeView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var nodeEntries = new List<NodeEntry>();

            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
                assembly => assembly.GetTypes()).Where(type => typeof(NodeView).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract &&
                                                               type != typeof(NodeView) && type != typeof(ResultNodeView)).ToArray();
            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(TitleAttribute), false) is TitleAttribute[] attrs && attrs.Length > 0)
                {
                    var node = (NodeView)Activator.CreateInstance(type);
                    nodeEntries.Add(new NodeEntry
                    {
                        nodeView = node,
                        title = attrs[0].title
                    });
                }
            }

            //* Build up the data structure needed by SearchWindow.
            // `groups` contains the current group path we're in.
            var groups = new List<string>();

            // First item in the tree is the title of the window.
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            foreach (var nodeEntry in nodeEntries)
            {
                // `createIndex` represents from where we should add new group entries from the current entry's group path.
                var createIndex = int.MaxValue;

                // Compare the group path of the current entry to the current group path.
                for (var i = 0; i < nodeEntry.title.Length - 1; i++)
                {
                    var group = nodeEntry.title[i];
                    if (i >= groups.Count)
                    {
                        // The current group path matches a prefix of the current entry's group path, so we add the
                        // rest of the group path from the currrent entry.
                        createIndex = i;
                        break;
                    }
                    if (groups[i] != group)
                    {
                        // A prefix of the current group path matches a prefix of the current entry's group path,
                        // so we remove everyfrom from the point where it doesn't match anymore, and then add the rest
                        // of the group path from the current entry.
                        groups.RemoveRange(i, groups.Count - i);
                        createIndex = i;
                        break;
                    }
                }

                // Create new group entries as needed.
                // If we don't need to modify the group path, `createIndex` will be `int.MaxValue` and thus the loop won't run.
                for (var i = createIndex; i < nodeEntry.title.Length - 1; i++)
                {
                    var group = nodeEntry.title[i];
                    groups.Add(group);
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(group)) { level = i + 1 });
                }

                // Finally, add the actual entry.
                tree.Add(new SearchTreeEntry(new GUIContent(nodeEntry.title.Last(), m_Icon)) { level = nodeEntry.title.Length, userData = nodeEntry });
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            var nodeEntry = (NodeEntry)entry.userData;
            var nodeView = nodeEntry.nodeView;
            nodeView.node.name = nodeEntry.title[nodeEntry.title.Length - 1];
            Vector2 worldMousePosition = context.screenMousePosition - position.position;
            Vector2 mousePosition = _nodeGraphView.contentViewContainer.WorldToLocal(worldMousePosition);
            nodeView.node.Guid = GUID.Generate().ToString();
            nodeView.node.Position = mousePosition;
            nodeView.viewDataKey = nodeView.node.Guid;
            nodeView.style.left = mousePosition.x;
            nodeView.style.top = mousePosition.y;
            _nodeGraph.AddNode(nodeView.node);
            _nodeGraphView.AddNodeView(nodeView);
            return true;
        }
    }
}