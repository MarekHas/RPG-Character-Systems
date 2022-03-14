using System;
using System.Collections.Generic;
using LevelUpSystem;
using LevelUpSystem.Nodes;
using UnityEngine;

namespace StatsSystem
{
    [RequireComponent(typeof(ICanLevelUp))]
    public class CharacterStatsController : StatsController
    {
        protected ICanLevelUp CanLevelUp;
        protected int _StatPoints = 5;

        public event Action WtatPointsChanged;

        public int statPoints
        {
            get => _StatPoints;
            internal set
            {
                _StatPoints = value;
                WtatPointsChanged?.Invoke();
            }
        }

        protected override void Awake()
        {
            CanLevelUp = GetComponent<ICanLevelUp>();
        }

        private void OnEnable()
        {
            CanLevelUp.Initialized += OnLevelableInitialized;
            CanLevelUp.WillUninitialize += UnregisterEvents;

            if (CanLevelUp.IsInitialized)
            {
                OnLevelableInitialized();
            }
        }

        private void OnDisable()
        {
            CanLevelUp.Initialized -= OnLevelableInitialized;
            CanLevelUp.WillUninitialize -= UnregisterEvents;

            if (CanLevelUp.IsInitialized)
            {
                UnregisterEvents();
            }
        }

        private void OnLevelableInitialized()
        {
            Initialize();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            CanLevelUp.LevelChanged += OnLevelChanged;
        }

        private void UnregisterEvents()
        {
            CanLevelUp.LevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            statPoints += 5;
        }

        protected override void InitializeStatFormulas()
        {
            base.InitializeStatFormulas();
            foreach (Stat currentStat in _stats.Values)
            {
                if (currentStat.Definition.Formula != null && currentStat.Definition.Formula.RootNode != null)
                {
                    List<LevelNode> levelNodes = currentStat.Definition.Formula.FindNodesOfType<LevelNode>();
                    foreach (LevelNode levelNode in levelNodes)
                    {
                        levelNode.levelable = CanLevelUp;
                        CanLevelUp.LevelChanged += currentStat.CalculateValue;
                    }
                }
            }
        }
    }
}