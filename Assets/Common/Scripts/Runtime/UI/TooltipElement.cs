using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Common.UI
{
    public class TooltipElement : VisualElement
    {
        private int _duration = 1000;//miliseconds

        public new class UxmlFactory : UxmlFactory<TooltipElement, UxmlTraits> { }

        public void Show()
        {
            style.display = DisplayStyle.Flex;
            experimental.animation.Start(new StyleValues { opacity = 0f },
                new StyleValues { opacity = 1f }, _duration).Ease(Easing.OutQuad);
        }

        public void Hide()
        {
            experimental.animation.Start(new StyleValues { opacity = 1f },
                new StyleValues { opacity = 0f }, _duration).Ease(Easing.OutQuad);
            style.display = DisplayStyle.None;
        }
    }
}