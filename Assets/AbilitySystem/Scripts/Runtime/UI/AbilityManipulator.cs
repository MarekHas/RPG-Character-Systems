using UnityEngine.UIElements;

namespace AbilitySystem.UI
{
    public class AbilityManipulator : Manipulator
    {
        private AbilityTooltipElement _tooltipElement;
        private Ability _ability;

        public AbilityManipulator(Ability ability, AbilityTooltipElement tooltipElement)
        {
            _ability = ability;
            _tooltipElement = tooltipElement;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            HideTooltip();
        }

        private void OnMouseEnter(MouseEnterEvent evt)
        {
            ShowTooltip();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            _tooltipElement.style.left = evt.mousePosition.x;
            _tooltipElement.style.top = evt.mousePosition.y;
        }

        private void ShowTooltip()
        {
            Label title = _tooltipElement.Q<Label>("ability-tooltip__title");
            title.text = _ability.AbilityDescription.name;
            Label description = _tooltipElement.Q<Label>("ability-tooltip__description");
            description.text = _ability.AbilityDescription.Description;
            VisualElement icon = _tooltipElement.Q("ability-tooltip__icon");
            icon.style.backgroundImage = new StyleBackground(_ability.AbilityDescription.Icon);
            Label data = _tooltipElement.Q<Label>("ability-tooltip__data");
            data.text = _ability.ToString();
            _tooltipElement.Show();
        }

        private void HideTooltip()
        {
            _tooltipElement.Hide();
        }
    }
}