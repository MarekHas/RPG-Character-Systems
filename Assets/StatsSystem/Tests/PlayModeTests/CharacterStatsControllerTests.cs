using System.Collections;
using LevelUpSystem;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatsSystem.Tests
{
    public class CharacterStatsControllerTests : MonoBehaviour
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatsSystem/Tests/Scenes/PlaymodeTestsScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator PlayerStatController_WhenLevelUp_GainStatPoints()
        {
            yield return null;
            CharacterStatsController playerStatController = GameObject.FindObjectOfType<CharacterStatsController>();
            ICanLevelUp levelable = playerStatController.GetComponent<ICanLevelUp>();
            Assert.AreEqual(5, playerStatController.statPoints);
            Assert.AreEqual(1, levelable.Level);
            levelable.CurrentExperience += 100;
            Assert.AreEqual(2, levelable.Level);
            Assert.AreEqual(10, playerStatController.statPoints);
        }
    }
}

