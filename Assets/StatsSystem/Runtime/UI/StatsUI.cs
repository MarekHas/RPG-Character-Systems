using LevelUpSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatsSystem.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private CharacterStatsController _characterStatsController;
        private UIDocument _document;
        private ICanLevelUp _canLevelUp;
        
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _canLevelUp = _characterStatsController.GetComponent<ICanLevelUp>();
        }

        private void Start()
        {
            var root = _document.rootVisualElement;

            VisualElement experience = root.Q("experience");
            Label experienceValue = experience.Q<Label>("value");

            experienceValue.text = $"{_canLevelUp.CurrentExperience} / {_canLevelUp.RequiredExperience}";
            
            _canLevelUp.ExperienceChanged += () =>
            {
                experienceValue.text = $"{_canLevelUp.CurrentExperience} / {_canLevelUp.RequiredExperience}";
            };

            VisualElement level = root.Q("level");
            Label levelValue = level.Q<Label>("value");
            levelValue.text = _canLevelUp.Level.ToString();
            
            _canLevelUp.LevelChanged += () =>
            {
                experienceValue.text = $"{_canLevelUp.CurrentExperience} / {_canLevelUp.RequiredExperience}";
                levelValue.text = _canLevelUp.Level.ToString();
            };

            VisualElement primaryStats = root.Q("primaryStats");
            
            for (int i = 0; i < primaryStats.childCount; i++)
            {
                Stat stat = _characterStatsController.Stats[primaryStats[i].name];
                Label label = primaryStats[i].Q<Label>("value");
                label.text = stat.Value.ToString();
                stat.ValueChanged += () =>
                {
                    label.text = stat.Value.ToString();
                };
                Button incrementButton = primaryStats[i].Q<Button>("add");
                incrementButton.SetEnabled(_characterStatsController.statPoints > 0 && stat.BaseValue != stat.Definition.Cap);
                incrementButton.clicked += () =>
                {
                    (stat as PrimaryStat).Add(1);
                    label.text = stat.Value.ToString();
                    incrementButton.SetEnabled(stat.BaseValue != stat.Definition.Cap);
                    _characterStatsController.statPoints--;
                };
            }

            VisualElement stats = root.Q("stats");
            
            for (int i = 0; i < stats.childCount; i++)
            {
                Stat stat = _characterStatsController.Stats[stats[i].name];//.Replace("-", "")];
                Label label = stats[i].Q<Label>("value");
                label.text = stat.Value.ToString();
                stat.ValueChanged += () =>
                {
                    label.text = stat.Value.ToString();
                };
            }

            VisualElement statPoints = root.Q("statPoints");
            Label statPointsValue = statPoints.Q<Label>("value");
            statPointsValue.text = _characterStatsController.statPoints.ToString();
            
            _characterStatsController.WtatPointsChanged += () =>
            {
                statPointsValue.text = _characterStatsController.statPoints.ToString();
                for (int i = 0; i < primaryStats.childCount; i++)
                {
                    Button incrementButton = primaryStats[i].Q<Button>("add");
                    incrementButton.SetEnabled(_characterStatsController.statPoints > 0);
                }
            };
        }

        public void Show()
        {
            _document.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _document.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}