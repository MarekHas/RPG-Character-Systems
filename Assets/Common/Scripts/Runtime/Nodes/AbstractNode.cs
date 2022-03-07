using UnityEngine;

namespace Common.Nodes
{
    public abstract class AbstractNode : ScriptableObject
    {
        [HideInInspector] public Vector2 Position;
        [HideInInspector] public string Guid;
    }
}