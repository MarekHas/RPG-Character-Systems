using Common.Editor;
using UnityEngine.UIElements;

namespace StatsSystem.Editor
{
    public class StatsCollectionEditor : ScriptableObjectCollectionEditor<StatDefinition>
    {
        public new class UxmlFactory : UxmlFactory<StatsCollectionEditor, UxmlTraits> { }
    }
}
