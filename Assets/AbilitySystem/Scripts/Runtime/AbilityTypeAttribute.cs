using System;

namespace AbilitySystem
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AbilityTypeAttribute : Attribute
    {
        public readonly Type Type;

        public AbilityTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}