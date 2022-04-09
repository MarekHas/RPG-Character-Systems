using System.Collections;
using LevelUpSystem;
using NUnit.Framework;
using StatsSystem.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace StatsSystem.Tests
{
    public class CharacterProgressDisplayTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatsSystem/Tests/Scenes/PlaymodeTestsScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Test01_text_refreshed_when_get_experience()
        {
            yield return null;
            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            ICanLevelUp levelable = playerStatController.GetComponent<ICanLevelUp>();
            CharacterProgressDisplayController headsUpDisplayUI = GameObject.FindObjectOfType<CharacterProgressDisplayController>();
            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            ProgressBar experienceBar = uiDocument.rootVisualElement.Q<ProgressBar>("experience");
            
            Assert.AreEqual(0, experienceBar.value);
            levelable.CurrentExperience += 5;
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(6, experienceBar.value, 0.5f);
        }

        [UnityTest]
        public IEnumerator Test02_Text_refresh_when_level_up()
        {
            yield return null;
            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            ICanLevelUp levelable = playerStatController.GetComponent<ICanLevelUp>();
            CharacterProgressDisplayController headsUpDisplayUI = GameObject.FindObjectOfType<CharacterProgressDisplayController>();
            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            Label level = uiDocument.rootVisualElement.Q<Label>("level");
            
            Assert.AreEqual("1", level.text);
            levelable.CurrentExperience += 100;
            Assert.AreEqual("2", level.text);
        }

        

        [UnityTest]
        public IEnumerator Test03_upadate_progress_bar_state()
        {
            yield return null;
            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            CharacterProgressDisplayController headsUpDisplayUI = GameObject.FindObjectOfType<CharacterProgressDisplayController>();

            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            ProgressBar manaBar = uiDocument.rootVisualElement.Q<ProgressBar>("mana");
            Assert.AreEqual(100, manaBar.value);
            
            Attribute mana = playerStatController.Stats["Mana"] as Attribute;
            mana.ApplyModifier(new StatModifier
            {
                Magnitude = -10,
                Type = ModifierOperationType.Additive
            });
            UnityEngine.Assertions.Assert.AreEqual(90, manaBar.value);
        }
    }
}