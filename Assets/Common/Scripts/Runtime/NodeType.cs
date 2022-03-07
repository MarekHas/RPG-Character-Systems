using System;

namespace Common.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeType : Attribute
    {
        public readonly Type Type;

        public NodeType(Type type)
        {
            Type = type;
        }
    }
}
