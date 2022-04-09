using Common.Runtime;
using LevelUpSystem.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;
using SavingSystem.Scripts.Runtime;

namespace LevelUpSystem
{
    public class LevelUpController : MonoBehaviour, ICanLevelUp, ISavable
    {
        public int Level => _level;
        public int RequiredExperience => Mathf.RoundToInt(_requiredExperienceFormula.RootNode.Value);
        public bool IsInitialized => _isInitialized;
        public event Action Initialized;
        public event Action WillUninitialize;
        public event Action LevelChanged;
        public event Action ExperienceChanged;
        public event Action Loaded;

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
                    _currentExperience = value;
                    LevelUp();
                }
                else if (value < RequiredExperience)
                {
                    _currentExperience = value;
                    ExperienceChanged?.Invoke();
                }
            }
        }
        private void LevelUp()
        {
            _currentExperience -= RequiredExperience;
            ExperienceChanged?.Invoke();
            _level++;
            
            LevelChanged?.Invoke();

            if (_currentExperience >= RequiredExperience)
                LevelUp();
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
                levelNode.CanLevelUP = this;
            }

            _isInitialized = true;
            Initialized?.Invoke();
        }

        private void OnDestroy()
        {
            WillUninitialize?.Invoke();
        }

        #region Save System

        public object Data => new LevelControllerData
        {
            level = _level,
            currentExperience = _currentExperience
        };
        public void Load(object data)
        {
            LevelControllerData levelControllerData = (LevelControllerData)data;
            _currentExperience = levelControllerData.currentExperience;
            _level = levelControllerData.level;

            Loaded?.Invoke();
        }

        [Serializable]
        protected class LevelControllerData
        {
            public int level;
            public int currentExperience;
        }

        #endregion
    }
}

