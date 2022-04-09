using System.Collections.Generic;
using UnityEngine;
using Common.Nodes;
using UnityEditor;

namespace Common.Runtime
{
    [CreateAssetMenu(fileName = "NodeGraph", menuName = "Common/NodeGraph", order = 0)]
    public class NodeGraph : ScriptableObject
    {
        public FunctionNode RootNode;
        public List<FunctionNode> Nodes = new List<FunctionNode>();

        public float CalculateValue(GameObject source)
        {
            return RootNode.CalculateValue(source);
        }

        public List<T> FindNodesOfType<T>()
        {
            List<T> nodesOfType = new List<T>();

            foreach (FunctionNode node in Nodes)
            {
                if (node is T nodeOfType)
                {
                    nodesOfType.Add(nodeOfType);
                }
            }

            return nodesOfType;
        }

        public void AddNode(FunctionNode node)
        {
            Nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void DeleteNode(FunctionNode node)
        {
            Nodes.Remove(node);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void RemoveChild(FunctionNode parent, FunctionNode child, string portName)
        {
            if (parent is IntermediateNode intermediateNode)
            {
                intermediateNode.RemoveChild(child, portName);
                EditorUtility.SetDirty(intermediateNode);
            }
            else if (parent is ResultNode resultNode)
            {
                resultNode.child = null;
                EditorUtility.SetDirty(resultNode);
            }
        }

        public void AddChild(FunctionNode parent, FunctionNode child, string portName)
        {
            if (parent is IntermediateNode intermediateNode)
            {
                intermediateNode.AddChild(child, portName);
                EditorUtility.SetDirty(intermediateNode);
            }
            else if (parent is ResultNode resultNode)
            {
                resultNode.child = child;
                EditorUtility.SetDirty(resultNode);
            }
        }
    }
}
