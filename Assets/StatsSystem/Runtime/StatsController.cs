using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatsSystem.Nodes;
using SavingSystem.Scripts.Runtime;

namespace StatsSystem
{
    public class StatsController : MonoBehaviour, ISavable
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
            }
        }

        private void OnDestroy()
        {
            WillUninitialize?.Invoke();
        }

        protected void Initialize()
        {
            foreach (StatDefinition definition in _statDatabase.Stats)
            {
                _stats.Add(definition.name, new Stat(definition,this));
            }

            foreach (StatDefinition definition in _statDatabase.Attributes)
            {
                if (definition.name.Equals("Health", StringComparison.OrdinalIgnoreCase))
                {
                    _stats.Add(definition.name, new Health(definition, this));
                }
                else
                {
                    _stats.Add(definition.name, new Attribute(definition, this));
                }
            }

            foreach (StatDefinition definition in _statDatabase.PrimaryStats)
            {
                _stats.Add(definition.name, new PrimaryStat(definition,this));
            }

            InitializeStatFormulas();

            foreach (Stat stat in _stats.Values)
            {
                stat.Initialize();
            }

            _isInitialized = true;
            Initialized?.Invoke();
        }

        protected virtual void InitializeStatFormulas()
        {
            foreach (Stat currentStat in _stats.Values)
            {
                if (currentStat.Definition.Formula != null && currentStat.Definition.Formula.RootNode != null)
                {
                    List<StatNode> statNodes = currentStat.Definition.Formula.FindNodesOfType<StatNode>();

                    foreach (StatNode statNode in statNodes)
                    {
                        if (_stats.TryGetValue(statNode.StatName.Trim(), out Stat stat))
                        {
                            statNode.Stat = stat;
                            stat.ValueChanged += currentStat.CalculateValue;
                        }
                        else
                        {
                            Debug.LogWarning($"Stat {statNode.StatName.Trim()} does not exist!");
                        }
                    }
                }
            }
        }

        #region Stat System

        public virtual object Data
        {
            get
            {
                Dictionary<string, object> stats = new Dictionary<string, object>();
                foreach (Stat stat in _stats.Values)
                {
                    if (stat is ISavable savable)
                    {
                        stats.Add(stat.Definition.name, savable.Data);
                    }
                }

                return new StatControllerData
                {
                    stats = stats
                };
            }
        }
        public virtual void Load(object data)
        {
            StatControllerData statControllerData = (StatControllerData)data;
            foreach (Stat stat in _stats.Values)
            {
                if (stat is ISavable savable)
                {
                    savable.Load(statControllerData.stats[stat.Definition.name]);
                }
            }
        }

        [Serializable]
        protected class StatControllerData
        {
            public Dictionary<string, object> stats;
        }

        #endregion
    }
}
