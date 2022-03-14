using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatsSystem.Tests
{
    public class StatTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatsSystem/Tests/Scenes/PlaymodeTestsScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Stat_WhenModifierApplied_ChangesValue()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            Stat physicalAttack = statController.Stats["PhysicalAttack"];
            
            Assert.AreEqual(0, physicalAttack.Value);

            physicalAttack.AddModifier(new StatModifier
            {
                Magnitude = 5,
                Type = ModifierOperationType.Additive
            });

            Assert.AreEqual(5, physicalAttack.Value);
        }

        [UnityTest]
        public IEnumerator Stat_WhenModifierApplied_DoesNotExceedCap()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            Stat attackSpeed = statController.Stats["AttackSpeed"];

            Assert.AreEqual(1, attackSpeed.Value);
            
            attackSpeed.AddModifier(new StatModifier
            {
                Magnitude = 5,
                Type = ModifierOperationType.Additive
            });
            
            Assert.AreEqual(3, attackSpeed.Value);
        }

        [UnityTest]
        public IEnumerator Stat_WhenStrengthIncreased_UpdatePhysicalAttack()
        {
            yield return null;
            StatsController statController = GameObject.FindObjectOfType<StatsController>();
            PrimaryStat strength = statController.Stats["Strength"] as PrimaryStat;
            Stat physicalAttack = statController.Stats["PhysicalAttack"];
            Assert.AreEqual(1, strength.Value);
            Assert.AreEqual(3, physicalAttack.Value);
            strength.Add(3);
            Assert.AreEqual(12, physicalAttack.Value);
        }

    }
}