using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatsSystem.Tests
{
    public class PrimaryStatTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatsSystem/Tests/Scenes/PlaymodeTestsScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Stat_WhenAddCalled_ChangesBaseValue()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            PrimaryStat strength = statController.Stats["Strength"] as PrimaryStat;
            
            Assert.AreEqual(1, strength.Value);
            strength.Add(1);
            Assert.AreEqual(2, strength.Value);
        }
    }
}
