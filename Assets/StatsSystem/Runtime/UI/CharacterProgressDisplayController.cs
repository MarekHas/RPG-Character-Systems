using LevelUpSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatsSystem.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class CharacterProgressDisplayController : MonoBehaviour
    {
        [SerializeField] private CharacterStatsController _characterStatsController;
        private UIDocument _uiDocument;
        private ICanLevelUp _canLevelUp;
        private ProgressBar _healthProgressBar;
        private ProgressBar _manaProgressBar;
        private ProgressBar _experienceProgressBar;
        private Label _levelLabel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _canLevelUp = _characterStatsController.GetComponent<ICanLevelUp>();
        }

        private void OnEnable()
        {
            var root = _uiDocument.rootVisualElement;
            _healthProgressBar = root.Q<ProgressBar>("health");
            _manaProgressBar = root.Q<ProgressBar>("mana");
            _experienceProgressBar = root.Q<ProgressBar>("experience");
            _levelLabel = root.Q<Label>("level");
        }

        private void Start()
        {
            Attribute mana = _characterStatsController.Stats["Mana"] as Attribute;
            Attribute health = _characterStatsController.Stats["Health"] as Attribute;

            OnManaChangedInternal();
            OnHealthChangedInternal();
            OnLevelChanged();

            mana.ValueChanged += OnMaxManaChanged;
            mana.CurrentValueChanged += OnManaChanged;
            health.ValueChanged += OnMaxHealthChanged;
            health.CurrentValueChanged += OnHealthChanged;

            _canLevelUp.LevelChanged += OnLevelChanged;
            _canLevelUp.ExperienceChanged += OnCurrentExperienceChanged;
        }

        private void OnDestroy()
        {
            Attribute mana = _characterStatsController.Stats["Mana"] as Attribute;
            Attribute health = _characterStatsController.Stats["Health"] as Attribute;

            mana.ValueChanged -= OnMaxManaChanged;
            mana.CurrentValueChanged -= OnManaChanged;
            health.ValueChanged -= OnMaxHealthChanged;
            health.CurrentValueChanged -= OnHealthChanged;

            _canLevelUp.LevelChanged -= OnLevelChanged;
            _canLevelUp.ExperienceChanged -= OnCurrentExperienceChanged;
        }

        private void OnCurrentExperienceChanged()
        {
            OnExperienceChangedInternal();
        }

        private void OnLevelChanged()
        {
            OnExperienceChangedInternal();
            _levelLabel.text = _canLevelUp.Level.ToString();
        }

        private void OnExperienceChangedInternal()
        {
            _experienceProgressBar.value = (float)_canLevelUp.CurrentExperience / _canLevelUp.RequiredExperience * 100f;
            _experienceProgressBar.title = $"{_canLevelUp.CurrentExperience} / {_canLevelUp.RequiredExperience}";
        }

        private void OnHealthChanged()
        {
            OnHealthChangedInternal();
        }

        private void OnMaxHealthChanged()
        {
            OnHealthChangedInternal();
        }

        private void OnHealthChangedInternal()
        {
            Attribute health = _characterStatsController.Stats["Health"] as Attribute;

            _healthProgressBar.value = (float)health.CurrentValue / health.Value * 100f;
            _healthProgressBar.title = $"{health.CurrentValue} / {health.Value}";
        }

        private void OnManaChanged()
        {
            OnManaChangedInternal();
        }

        private void OnMaxManaChanged()
        {
            OnManaChangedInternal();
        }

        private void OnManaChangedInternal()
        {
            Attribute mana = _characterStatsController.Stats["Mana"] as Attribute;

            _manaProgressBar.value = (float)mana.CurrentValue / mana.Value * 100f;
            _manaProgressBar.title = $"{mana.CurrentValue} / {mana.Value}";
        }
    }
}