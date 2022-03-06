using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatsSystem.Tests
{
    public class AttributeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatsSystem/Tests/Scenes/PlaymodeTestsScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Attribute_WhenModifierApplied_DoesNotExceedMaxValue()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            Attribute health = statController.Stats["TestHealth"] as Attribute;
            
            Assert.AreEqual(100, health.CurrentValue);
            Assert.AreEqual(100, health.Value);
            
            health.ApplyModifier(new StatModifier
            {
                Magnitude = 20,
                Type = ModifierOperationType.Additive
            });
            
            Assert.AreEqual(100, health.CurrentValue);
        }

        [UnityTest]
        [CanBeNull]
        public IEnumerator Attribute_WhenModifierApplied_DoesNotGoBelowZero()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            Attribute health = statController.Stats["TestHealth"] as Attribute;
            Assert.AreEqual(100, health.CurrentValue);
            health.ApplyModifier(new StatModifier
            {
                Magnitude = -150,
                Type = ModifierOperationType.Additive
            });
            Assert.AreEqual(0, health.CurrentValue);
        }
    }
}