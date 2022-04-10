using System.Collections.Generic;
using System.Collections.ObjectModel;
using StatsSystem;
using UnityEngine;
using AbilitySystem.Scripts.Runtime;
using Common.Runtime;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AbilitySystem
{
    public class AbilityEffect: ITaggable
    {
        protected EffectData _effectData;
        private object _source;
        private GameObject _attacker;
        private List<StatModifier> _statModifiers = new List<StatModifier>();

        public EffectData EffectData => _effectData;
        public object Source => _source;
        public GameObject Attacker => _attacker;
        public ReadOnlyCollection<StatModifier> StatModifiers => _statModifiers.AsReadOnly();
        public ReadOnlyCollection<string> Tags => _effectData.Tags;

        public AbilityEffect(EffectData data, object source, GameObject attacker)
        {
            _effectData = data;
            _source = source;
            _attacker = attacker;

            StatsController statController = attacker.GetComponent<StatsController>();

            foreach (BaseStatModifierData modifierDefinition in data.ModifierDefinitions)
            {
                StatModifier statModifier;

                if (modifierDefinition is HealthModifierEffectData damageDefinition)
                {
                    HealthModifier healthModifier = new HealthModifier
                    {
                        Magnitude = Mathf.RoundToInt(modifierDefinition.Formula.CalculateValue(attacker)),
                        IsCriticalHit = false
                    };

                    if (damageDefinition.CanCriticalHit)
                    {
                        if (statController.Stats["CriticalHitChance"].Value / 100f >= Random.value)
                        {
                            healthModifier.Magnitude = Mathf.RoundToInt(healthModifier.Magnitude *
                                statController.Stats["CriticalHitMultiplier"].Value / 100f);
                            healthModifier.IsCriticalHit = true;
                        }
                    }

                    statModifier = healthModifier;
                }
                else
                {
                    statModifier = new StatModifier()
                    {
                        Magnitude = Mathf.RoundToInt(modifierDefinition.Formula.CalculateValue(attacker))
                    };
                }

                statModifier.DamageSource = this;
                statModifier.Type = modifierDefinition.ModifierType;

                _statModifiers.Add(statModifier);
            }
        }

        public override string ToString()
        {
            return ReplaceMacro(EffectData.Description, this);
        }

        protected string ReplaceMacro(string value, object @object)
        {
            return Regex.Replace(value, @"{(.+?)}", match =>
            {
                var p = Expression.Parameter(@object.GetType(), @object.GetType().Name);
                var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { p }, null,
                    match.Groups[1].Value);
                return (e.Compile().DynamicInvoke(@object) ?? "").ToString();
            });
        }
    }
}