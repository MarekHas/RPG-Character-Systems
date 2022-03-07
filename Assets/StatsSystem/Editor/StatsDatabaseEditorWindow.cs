using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatsSystem.Editor
{
    public class StatsDatabaseEditorWindow : EditorWindow
    {
        private StatsDatabase _statsDatabase;
        private StatsCollectionEditor _current;

        [MenuItem("Window/StatsSystem/StatDatabase")]
        public static void ShowWindow()
        {
            StatsDatabaseEditorWindow window = GetWindow<StatsDatabaseEditorWindow>();
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("StatsDatabase");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is StatsDatabase)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        private void OnSelectionChange()
        {
            _statsDatabase = Selection.activeObject as StatsDatabase;
        }

        public void CreateGUI()
        {
            OnSelectionChange();

            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/StatsSystem/Editor/StatsDatabaseEditorWindow.uxml");
            visualTree.CloneTree(root);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StatsSystem/Editor/StatsDatabaseEditorWindow.uss");
            root.styleSheets.Add(styleSheet);

            StatsCollectionEditor stats = root.Q<StatsCollectionEditor>("Stats");
            stats.Initialize(_statsDatabase, _statsDatabase.Stats);
            Button statsTab = root.Q<Button>("StatsTab");
            statsTab.clicked += () =>
            {
                _current.style.display = DisplayStyle.None;
                stats.style.display = DisplayStyle.Flex;
                _current = stats;
            };

            StatsCollectionEditor primaryStats = root.Q<StatsCollectionEditor>("PrimaryStats");
            primaryStats.Initialize(_statsDatabase, _statsDatabase.PrimaryStats);
            Button primaryStatsTab = root.Q<Button>("PrimaryStatsTab");
            primaryStatsTab.clicked += () =>
            {
                _current.style.display = DisplayStyle.None;
                primaryStats.style.display = DisplayStyle.Flex;
                _current = primaryStats;
            };

            StatsCollectionEditor attributes = root.Q<StatsCollectionEditor>("Attributes");
            attributes.Initialize(_statsDatabase, _statsDatabase.Attributes);
            Button attributesTab = root.Q<Button>("AttributesTab");
            attributesTab.clicked += () =>
            {
                _current.style.display = DisplayStyle.None;
                attributes.style.display = DisplayStyle.Flex;
                _current = attributes;
            };

            _current = stats;
        }
    }
}
