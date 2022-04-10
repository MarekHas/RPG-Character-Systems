using System;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(EffectData))]
    public class GameplayEffectEditor : Editor
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
            root.Add(CreateGameplayEffectFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            return root;
        }

        protected virtual VisualElement CreateCoreFieldsGUI()
        {
            VisualElement root = new VisualElement();

            TextField description = new TextField
            {
                label = "Description",
                bindingPath = "_description",
                multiline = true
            };
            description.Bind(serializedObject);
            description.AddToClassList("description");
            root.Add(description);
            return root;
        }
        protected VisualElement CreateApplicationFieldsGUI()
        {
            VisualElement root = new VisualElement();
            ListView modifiers = new ListView
            {
                bindingPath = "_modifierDefinitions",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Modifiers"
            };
            modifiers.Bind(serializedObject);
            Button addButton = modifiers.Q<Button>("unity-list-view__add-button");
            addButton.clicked += AddButtonOnClicked;
            root.Add(modifiers);
            return root;
        }
        protected virtual VisualElement CreateGameplayEffectFieldsGUI()
        {
            VisualElement root = new VisualElement();

            ListView listView = new ListView()
            {
                bindingPath = "_conditionalEffects",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Conditional Effects"
            };
            listView.Bind(serializedObject);
            root.Add(listView);
            return root;
        }

        protected virtual VisualElement CreateSpecialEffectFieldsGUI()
        {
            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("_specialEffectDefinition")));

            return root;
        }
        protected virtual VisualElement CreateTagFieldsGUI()
        {
            VisualElement root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("_tags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_removeEffectsWithTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_applicationMustBePresentTags")));
            root.Add(new PropertyField(serializedObject.FindProperty("_applicationMustBeAbsentTags")));
            return root;
        }

        private void AddButtonOnClicked()
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(BaseStatModifierData).IsAssignableFrom(type) &&
                               type.IsClass && !type.IsAbstract).ToArray();
            if (types.Length > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, delegate
                    {
                        CreateItem(type);
                    });
                }
                menu.ShowAsContext();
            }
            else
            {
                CreateItem(types[0]);
            }
        }

        private void CreateItem(Type type)
        {
            BaseStatModifierData item = ScriptableObject.CreateInstance(type) as BaseStatModifierData;
            item.name = "Modifier";
            AssetDatabase.AddObjectToAsset(item, target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SerializedProperty modifiers = serializedObject.FindProperty("_modifierDefinitions"); 
            modifiers.GetArrayElementAtIndex(modifiers.arraySize - 1).objectReferenceValue = item;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}