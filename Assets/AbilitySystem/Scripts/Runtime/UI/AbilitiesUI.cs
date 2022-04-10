using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class AbilitiesUI : MonoBehaviour
    {
        [SerializeField] private PlayerAbilityController _controller;

        private UIDocument _document;
        private VisualElement _parent;
        private Button _closeButton;
        private Label _abilityPoints;
        private AbilityTooltipElement _tooltipElement;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        private void Start()
        {
            var root = _document.rootVisualElement;
            _parent = root.Q("abilities__content");
            _tooltipElement = root.Q<AbilityTooltipElement>();
            _closeButton = root.Q<Button>("abilities__closeButton");
            _closeButton.clicked += Hide;

            foreach (Ability ability in _controller.Abilities.Values)
            {
                AbilityElement abilityElement = new AbilityElement
                {
                    name = ability.AbilityDescription.name
                };
                Label level = abilityElement.Q<Label>("ability__level");
                Label title = abilityElement.Q<Label>("ability__title");
                Button incrementButton = abilityElement.Q<Button>("ability__addButton");
                VisualElement icon = abilityElement.Q("ability__icon");

                level.text = ability.Level.ToString();
                title.text = ability.AbilityDescription.name;
                
                incrementButton.SetEnabled(_controller.AbilityPoints > 0 && ability.Level != ability.AbilityDescription.MaximumLevel);
                incrementButton.clicked += () =>
                {
                    ability.Level++;
                    level.text = ability.Level.ToString();
                    _controller.AbilityPoints--;
                };
                
                icon.style.backgroundImage = new StyleBackground(ability.AbilityDescription.Icon);
                abilityElement.AddManipulator(new AbilityManipulator(ability, _tooltipElement));
                _parent.Add(abilityElement);
            }

            _abilityPoints = root.Q<Label>("abilities__abilityPointsValue");
            OnAbilityPointsChanged();
            _controller.AbilityPointsChanged += OnAbilityPointsChanged;
        }

        private void OnAbilityPointsChanged()
        {
            _abilityPoints.text = _controller.AbilityPoints.ToString();
            for (int i = 0; i < _parent.childCount; i++)
            {
                Ability ability = _controller.Abilities[_parent[i].name];
                Button incrementButton = _parent[i].Q<Button>("ability__addButton");
                incrementButton.SetEnabled(_controller.AbilityPoints > 0 && ability.Level != ability.AbilityDescription.MaximumLevel);
            }
        }

        public void Hide()
        {
            _document.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            _document.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}