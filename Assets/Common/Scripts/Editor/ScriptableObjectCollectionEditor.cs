using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Common.Editor
{
    public class ScriptableObjectCollectionEditor<T> : VisualElement where T : ScriptableObject
    {
        protected ScriptableObject _target;
        protected List<T> _items;
        private ListView _listView;
        private Button _createButton;
        private List<T> _filteredListView;
        private InspectorElement _inspector;
        private ToolbarSearchField _toolbarSearchField;
        private PropertyField _nameField;

        public ScriptableObjectCollectionEditor()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Common/Scripts/Editor/ScriptableObjectCollectionEditor.uxml");
            visualTree.CloneTree(this);

            _inspector = this.Q<Inspector>();
            _listView = this.Q<ListView>();
            _toolbarSearchField = this.Q<ToolbarSearchField>();
            _createButton = this.Q<Button>("CreateButton");
            _nameField = this.Q<PropertyField>("NameField");
        }

        public void Initialize(ScriptableObject target, List<T> items)
        {
            _target = target;
            _items = items;
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            Func<VisualElement> makeItem = () => new Label();
            _listView.makeItem = makeItem;
            
            _listView.onSelectionChange += objects =>
            {
                T item = objects.First() as T;
                Select(item);
            };

            Action<VisualElement, int> bindItem = (element, i) =>
            {
                Label label = element as Label;

                label.AddManipulator(new ContextualMenuManipulator(contextMenuPopulateEvent =>
                {
                    contextMenuPopulateEvent.menu.AppendAction("Duplicate", action =>
                    {
                        Duplicate(_filteredListView[i]);
                    });
                    contextMenuPopulateEvent.menu.AppendAction("Remove", action =>
                    {
                        Remove(_filteredListView[i]);
                    });
                }));

                SerializedObject serializedObject = new SerializedObject(_filteredListView[i]);
                SerializedProperty serializedProperty = serializedObject.FindProperty("m_Name");
                label.BindProperty(serializedProperty);
            };

            _listView.bindItem = bindItem;
            _listView.itemsSource = _filteredListView = _items;

            _createButton.clicked += Create;

            _toolbarSearchField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                _listView.itemsSource = _filteredListView = _items
                    .Where(item => item.name.StartsWith(evt.newValue, StringComparison.OrdinalIgnoreCase)).ToList();
                _listView.Rebuild();
            });
        }

        private void Duplicate(T item)
        {
            T duplicate = ScriptableObject.Instantiate(item);
            duplicate.hideFlags = HideFlags.HideInHierarchy;

            AssetDatabase.AddObjectToAsset(duplicate, _target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            _items.Add(duplicate);
            _listView.Rebuild();
            
            Select(duplicate);
            
            _listView.SetSelection(_items.Count - 1);
            EditorUtility.SetDirty(_target);
        }

        private void Create()
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            if (types.Length > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type type in types)
                {
                    menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(type.Name)), false, delegate
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
            T item = (T)ScriptableObject.CreateInstance(type);
            item.name = "New Item";
            item.hideFlags = HideFlags.HideInHierarchy;
            
            AssetDatabase.AddObjectToAsset(item, _target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            _items.Add(item);
            _listView.Rebuild();
            
            Select(item);
            
            _listView.SetSelection(_items.Count - 1);
            EditorUtility.SetDirty(_target);
        }

        private void Select(T item)
        {
            SerializedObject serializedObject = new SerializedObject(item);
            _inspector.Bind(serializedObject);
            _nameField.Bind(serializedObject);
        }

        private void Remove(T item)
        {
            if (EditorUtility.DisplayDialog("Delete Item", "Are you sure you want to delete item " + item.name + "?", "Yes",
                "No"))
            {
                ScriptableObject.DestroyImmediate(item, true);
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                _items.Remove(item);
                _listView.Rebuild();
                
                EditorUtility.SetDirty(_target);
            }
        }
    }
}
