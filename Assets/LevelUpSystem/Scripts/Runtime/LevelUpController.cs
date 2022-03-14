using Common.Runtime;
using LevelUpSystem.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelUpSystem
{
    public class LevelUpController : MonoBehaviour, ICanLevelUp
    {
        public int Level => _level;
        public int RequiredExperience => Mathf.RoundToInt(_requiredExperienceFormula.RootNode.Value);
        public bool IsInitialized => _isInitialized;
        public event Action Initialized;
        public event Action WillUninitialize;
        public event Action LevelChanged;
        public event Action ExperienceChanged;

        [SerializeField] private int _level = 1;
        [SerializeField] private int _currentExperience;
        [SerializeField] private NodeGraph _requiredExperienceFormula;
        private bool _isInitialized;

        public int CurrentExperience
        {
            get => _currentExperience;
            set
            {
                if (value >= RequiredExperience)
                {
                    _currentExperience = value - RequiredExperience;
                    ExperienceChanged?.Invoke();

                    _level++;
                    LevelChanged?.Invoke();
                }
                else if (value < RequiredExperience)
                {
                    _currentExperience = value;
                    ExperienceChanged?.Invoke();
                }
            }
        }

        private void Awake()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            List<LevelNode> levelNodes = _requiredExperienceFormula.FindNodesOfType<LevelNode>();
            foreach (LevelNode levelNode in levelNodes)
            {
                levelNode.levelable = this;
            }

            _isInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDestroy()
        {
            WillUninitialize?.Invoke();
        }
    }
}

