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
            Stat physicalAttack = statController.Stats["TestPhysicalAttack"];
            
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
            Stat attackSpeed = statController.Stats["TestAttackSpeed"];

            Assert.AreEqual(1, attackSpeed.Value);
            
            attackSpeed.AddModifier(new StatModifier
            {
                Magnitude = 5,
                Type = ModifierOperationType.Additive
            });
            
            Assert.AreEqual(3, attackSpeed.Value);
        }
    }
}