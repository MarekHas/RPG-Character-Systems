using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(StackableEffectData))]
    public class GameplayStackableEffectEditor : GameplayPersistentEffectEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            StyleSheet styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Assets/AbilitySystem/Scripts/Editor/GameplayEffectEditor.uss");
            root.styleSheets.Add(styleSheet);

            root.Add(CreateCoreFieldsGUI());

            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateDurationFieldsGUI());
            root.Add(CreatePeriodFieldsGUI());
            root.Add(CreateStackingFieldsGUI());
            root.Add(CreateGameplayEffectFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            RegisterCallbacks(root);

            return root;
        }

        protected VisualElement CreateStackingFieldsGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("_denyOverflowApplication")));
            root.Add(new PropertyField(serializedObject.FindProperty("_clearStackOnOverflow")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackLimitCount")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackDurationRefreshPolicy")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackPeriodResetPolicy")));
            root.Add(new PropertyField(serializedObject.FindProperty("_stackExpirationPolicy")));

            return root;
        }

        protected override VisualElement CreateGameplayEffectFieldsGUI()
        {
            VisualElement root = base.CreateGameplayEffectFieldsGUI();

            ListView overflowGameplayEffects = new ListView
            {
                bindingPath = "_overflowEffects",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Overflow Gameplay Effects"
            };
            overflowGameplayEffects.Bind(serializedObject);
            root.Add(overflowGameplayEffects);
            return root;
        }
    }
}