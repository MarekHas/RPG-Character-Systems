using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Common.Editor
{
    public class Inspector : InspectorElement
    {
        public new class UxmlFactory : UxmlFactory<Inspector, UxmlTraits> { }
    }
}
