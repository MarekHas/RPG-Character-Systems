using System;
using LevelUpSystem;
using UnityEngine;

namespace AbilitySystem
{
    [RequireComponent(typeof(ICanLevelUp))]
    public class PlayerAbilityController : AbilityController
    {
        protected ICanLevelUp _canLevelUp;
        protected int _abilityPoints;

        public event Action AbilityPointsChanged;

        public int AbilityPoints
        {
            get => _abilityPoints;
            internal set
            {
                _abilityPoints = value;
                AbilityPointsChanged?.Invoke();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _canLevelUp = GetComponent<ICanLevelUp>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _canLevelUp.Initialized += OnLevelableInitialized;
            _canLevelUp.WillUninitialize += UnregisterEvents;
            
            if (_canLevelUp.IsInitialized)
            {
                OnLevelableInitialized();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _canLevelUp.Initialized -= OnLevelableInitialized;
            _canLevelUp.WillUninitialize -= UnregisterEvents;
            
            if (_canLevelUp.IsInitialized)
            {
                UnregisterEvents();
            }
        }

        private void OnLevelableInitialized()
        {
            RegisterEvents();
        }

        private void UnregisterEvents()
        {
            _canLevelUp.LevelChanged -= OnLevelChanged;
        }

        private void RegisterEvents()
        {
            _canLevelUp.LevelChanged += OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            AbilityPoints += 3;
        }

        #region Save System

        public override object Data
        {
            get
            {
                return new PlayerAbilityControllerData(base.Data as AbilityControllerData)
                {
                    AbilityPoints = _abilityPoints
                };
            }
        }

        public override void Load(object data)
        {
            base.Load(data);

            PlayerAbilityControllerData playerAbilityControllerData = (PlayerAbilityControllerData)data;
            _abilityPoints = playerAbilityControllerData.AbilityPoints;
            AbilityPointsChanged?.Invoke();
        }

        [Serializable]
        protected class PlayerAbilityControllerData : AbilityControllerData
        {
            public int AbilityPoints;

            public PlayerAbilityControllerData(AbilityControllerData abilityControllerData)
            {
                Abilities = abilityControllerData.Abilities;
            }
        }

        #endregion
    }
}