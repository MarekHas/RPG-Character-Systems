using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    public class StatsController : MonoBehaviour
    {
        [SerializeField] private StatsDatabase _statDatabase;
        protected Dictionary<string, Stat> _stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Stat> Stats => _stats;

        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;
        public event Action Initialized;
        public event Action WillUninitialize;

        protected virtual void Awake()
        {
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
                Initialized?.Invoke();
            }
        }

        private void OnDestroy()
        {
            WillUninitialize?.Invoke();
        }

        private void Initialize()
        {
            foreach (StatDefinition definition in _statDatabase.Stats)
            {
                _stats.Add(definition.name, new Stat(definition));
            }

            foreach (StatDefinition definition in _statDatabase.Attributes)
            {
                _stats.Add(definition.name, new Attribute(definition));
            }

            foreach (StatDefinition definition in _statDatabase.PrimaryStats)
            {
                _stats.Add(definition.name, new PrimaryStat(definition));
            }
        }
    }
}
