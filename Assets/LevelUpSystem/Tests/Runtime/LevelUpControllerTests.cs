using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LevelUpSystem.Tests
{
    public class LevelControllerTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/LevelUpSystem/Tests/Scenes/TestLevelUpSystem.unity",
                new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator LevelController_WhenLevelUp_UpdateRequiredExperience()
        {
            yield return null;
            LevelUpController levelController = GameObject.FindObjectOfType<LevelUpController>();
            Assert.AreEqual(1, levelController.Level);
            levelController.CurrentExperience += 83;
            Assert.AreEqual(2, levelController.Level);
            Assert.AreEqual(0, levelController.CurrentExperience);
            Assert.AreEqual(92, levelController.RequiredExperience);
        }
    }
}