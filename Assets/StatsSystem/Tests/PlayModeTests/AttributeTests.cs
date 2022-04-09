using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Runtime;

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
            Attribute health = statController.Stats["Health"] as Attribute;
            
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
            Attribute health = statController.Stats["Health"] as Attribute;
            Assert.AreEqual(100, health.CurrentValue);
            health.ApplyModifier(new StatModifier
            {
                Magnitude = -150,
                Type = ModifierOperationType.Additive
            });
            Assert.AreEqual(0, health.CurrentValue);
        }

        [UnityTest]
        public IEnumerator Attribute_WhenTakeDamage_DamageReducedByDefense()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            Health health = statController.Stats["Health"] as Health;
            Assert.AreEqual(100, health.CurrentValue);
            health.ApplyModifier(new HealthModifier
            {
                Magnitude = -10,
                Type = ModifierOperationType.Additive,
                IsCriticalHit = false,
                DamageSource = ScriptableObject.CreateInstance<Ability>()
            });
            Assert.AreEqual(90, health.CurrentValue);
        }

        private class Ability : ScriptableObject, ITaggable
        {
            private List<string> m_Tags = new List<string>() { "physical" };
            public ReadOnlyCollection<string> Tags => m_Tags.AsReadOnly();
        }
    }
}