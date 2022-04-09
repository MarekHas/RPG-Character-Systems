using System.Collections;
using LevelUpSystem;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace StatsSystem.Tests
{
    public class StatsUITests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatsSystem/Tests/Scenes/PlaymodeTestsScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Test01_base_value_equal_one()
        {
            yield return null;
            
            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            Assert.AreEqual(1, playerStatController.Stats["Strength"].Value);
        }

        [UnityTest]
        public IEnumerator Test02_Add_button_increment_base_value_by_one()
        {
            yield return null;

            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            int baseValue = playerStatController.Stats["Strength"].Value;

            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement strengthElement = uiDocument.rootVisualElement.Q("strength");
            Button addStatPointButton = strengthElement.Q<Button>("add");

            using (var e = new NavigationSubmitEvent { target = addStatPointButton })
            {
                addStatPointButton.SendEvent(e);
            }

            Assert.AreEqual(baseValue + 1, playerStatController.Stats["Strength"].Value);
        }

        [UnityTest]
        public IEnumerator Test03_Decreasing_stat_points_value_when_primary_stat_added()
        {
            yield return null;

            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            var baseStatPointsValue = playerStatController.statPoints;

            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement strengthElement = uiDocument.rootVisualElement.Q("strength");
            Button addStatPointButton = strengthElement.Q<Button>("add");
            
            using (var e = new NavigationSubmitEvent { target = addStatPointButton })
            {
                addStatPointButton.SendEvent(e);
            }
            
            Assert.AreEqual(baseStatPointsValue - 1, playerStatController.statPoints);
        }

        [UnityTest]
        public IEnumerator Test04_Add_buttons_disabaled_when_zero_stat_points()
        {
            yield return null;

            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            var baseStatPointsValue = playerStatController.statPoints;
            var baseStrengthValue = playerStatController.Stats["Strength"].Value;

            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement strengthElement = uiDocument.rootVisualElement.Q("strength");
            Button addStatPointButton = strengthElement.Q<Button>("add");
            
            for (int i = 0; i < baseStatPointsValue; i++)
            {
                using (var e = new NavigationSubmitEvent { target = addStatPointButton })
                {
                    addStatPointButton.SendEvent(e);
                }
            }
            
            Assert.AreEqual(0, playerStatController.statPoints);
            Assert.AreEqual(false, addStatPointButton.enabledSelf);
            Assert.AreEqual(playerStatController.statPoints + playerStatController.Stats["Strength"].Value, baseStatPointsValue + baseStrengthValue);
        }

        [UnityTest]
        public IEnumerator Test05_Refresh_text_when_stat_changed()
        {
            yield return null;

            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement physicalAttackElement = uiDocument.rootVisualElement.Q("physicalAttack");
            Label physicalAttackValue = physicalAttackElement.Q<Label>("value");
            var baseValue = playerStatController.Stats["PhysicalAttack"].Value;

            Assert.AreEqual(baseValue.ToString(), physicalAttackValue.text);
            int addedValue = 5;

            playerStatController.Stats["PhysicalAttack"].AddModifier(new StatModifier
            {
                Magnitude = addedValue,
                Type = ModifierOperationType.Additive
            });

            var currentValue = baseValue + addedValue;
            Assert.AreEqual(currentValue.ToString(), physicalAttackValue.text);
        }

        [UnityTest]
        public IEnumerator Test06_Add_button_disabled_when_stat_capacity_reached()
        {
            yield return null;

            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement statElement = uiDocument.rootVisualElement.Q("constitution");
            Button addStatPointButton = statElement.Q<Button>("add");
           
            Assert.AreEqual(false, addStatPointButton.enabledSelf);
        }

        [UnityTest]
        public IEnumerator Test07_Text_refreshed_when_level_up()
        {
            yield return null;

            LevelUpController levelController = GameObject.FindObjectOfType<LevelUpController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement levelElement = uiDocument.rootVisualElement.Q("level");
            Label levelValue = levelElement.Q<Label>("value");
            Assert.AreEqual("1", levelValue.text);
            levelController.CurrentExperience += 100;
            Assert.AreEqual("2", levelValue.text);
        }

        [UnityTest]
        public IEnumerator Test08_Text_refreshed_when_experience_points_added()
        {
            yield return null;

            LevelUpController levelController = GameObject.FindObjectOfType<LevelUpController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement experienceElement = uiDocument.rootVisualElement.Q("experience");
            Label experienceValue = experienceElement.Q<Label>("value");
            Assert.AreEqual("17 / 92", experienceValue.text);
            levelController.CurrentExperience += 5;
            Assert.AreEqual("22 / 92", experienceValue.text);
        }
    }
}