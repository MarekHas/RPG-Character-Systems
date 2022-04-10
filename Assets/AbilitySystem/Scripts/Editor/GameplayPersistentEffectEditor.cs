using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(PersistentEffectData))]
    public class GameplayPersistentEffectEditor : GameplayEffectEditor
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
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            RegisterCallbacks(root);

            return root;
        }

        protected void RegisterCallbacks(VisualElement root)
        {
            PersistentEffectData definition = target as PersistentEffectData;

            PropertyField durationField = root.Q<PropertyField>("duration-formula");
            PropertyField isInfiniteField = root.Q<PropertyField>("is-infinite");

            durationField.style.display = definition.IsInfinite ? DisplayStyle.None : DisplayStyle.Flex;
            isInfiniteField.RegisterValueChangeCallback(evt =>
            {
                durationField.style.display = evt.changedProperty.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            });

            VisualElement periodFields = root.Q("period");
            PropertyField isPeriodicField = root.Q<PropertyField>("is-periodic");
            periodFields.style.display = definition.IsPeriodic ? DisplayStyle.Flex : DisplayStyle.None;

            isPeriodicField.RegisterValueChangeCallback(evt =>
            {
                periodFields.style.display = evt.changedProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }

        protected VisualElement CreateDurationFieldsGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("_isInfinite")) { name = "is-infinite" });
            root.Add(new PropertyField(serializedObject.FindProperty("_durationFormula")) { name = "duration-formula" });

            return root;
        }

        protected VisualElement CreatePeriodFieldsGUI()
        {
            VisualElement root = new VisualElement();

            VisualElement periodFields = new VisualElement() { name = "period" };

            periodFields.Add(new PropertyField(serializedObject.FindProperty("_period")));
            periodFields.Add(new PropertyField(serializedObject.FindProperty("_executePeriodicEffectOnApplication")));
            periodFields.Add(new PropertyField(serializedObject.FindProperty("_periodicInhibitionPolicy")));

            root.Add(new PropertyField(serializedObject.FindProperty("_isPeriodic")) { name = "is-periodic" });
            root.Add(periodFields);

            return root;
        }

        protected override VisualElement CreateSpecialEffectFieldsGUI()
        {
            VisualElement root = base.CreateSpecialEffectFieldsGUI();

            root.Add(new PropertyField(serializedObject.FindProperty("_specialPersistentEffectDefinition")));

            return root;
        }

        protected override VisualElement CreateTagFieldsGUI()
        {
            VisualElement root = base.CreateTagFieldsGUI();

            root.Add(new PropertyField(serializedObject.FindProperty("_grantedTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_grantedApplicationImmunityTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_uninhibitedMustBePresentTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_uninhibitedMustBeAbsentTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_persistMustBePresentTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_persistMustBeAbsentTags")));

            return root;
        }
    }
}