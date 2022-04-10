using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StatsSystem;
using UnityEngine;
using Attribute = StatsSystem.Attribute;
using Common.Runtime;
using AbilitySystem.Scripts.Runtime;

namespace AbilitySystem
{
    [RequireComponent(typeof(StatsController))]
    [RequireComponent(typeof(TagController))]
    public partial class GameplayEffectController : MonoBehaviour
    {
        [SerializeField] private List<EffectData> _startEffectDefinitions;
        private bool _isInitialized;
        
        protected TagController _tagController;
        protected StatsController _statController;
        protected List<PersistentEffect> _activeEffects = new List<PersistentEffect>();

        public event Action Initialized;
        
        public ReadOnlyCollection<PersistentEffect> ActiveEffects => _activeEffects.AsReadOnly();
        public bool IsInitialized => _isInitialized;
        
        private void Update()
        {
            HandleDuration();
        }
        private void Awake()
        {
            _statController = GetComponent<StatsController>();
            _tagController = GetComponent<TagController>();
        }

        private void OnEnable()
        {
            _tagController.TagAdded += CheckOngoingTagRequirements;
            _tagController.TagRemoved += CheckOngoingTagRequirements;
            _tagController.TagAdded += CheckRemovalTagRequirements;
            _tagController.TagRemoved += CheckRemovalTagRequirements;
            _statController.Initialized += OnStatControllerInitialized;

            if (_statController.IsInitialized)
            {
                OnStatControllerInitialized();
            }
        }
        private void OnDisable()
        {
            _tagController.TagAdded -= CheckOngoingTagRequirements;
            _tagController.TagRemoved -= CheckOngoingTagRequirements;
            _tagController.TagAdded -= CheckRemovalTagRequirements;
            _tagController.TagRemoved -= CheckRemovalTagRequirements;
        }
        private void OnStatControllerInitialized()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (EffectData effectDefinition in _startEffectDefinitions)
            {
                EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>().FirstOrDefault();

                AbilityEffect effect = Activator.CreateInstance(attribute.Type, effectDefinition, _startEffectDefinitions, gameObject) as AbilityEffect;
                ApplyGameplayEffectToSelf(effect);
            }

            _isInitialized = true;
            Initialized?.Invoke();
        }

        public bool ApplyGameplayEffectToSelf(AbilityEffect effectToApply)
        {

            foreach (PersistentEffect activeEffect in _activeEffects)
            {
                if (activeEffect.IsInhibited == false)
                {
                    foreach (string tag in activeEffect.EffectData.GrantedApplicationImmunityTags)
                    {
                        if (effectToApply.EffectData.Tags.Contains(tag))
                        {
                            Debug.Log($"Immune to {effectToApply.EffectData.name}");
                            return false;
                        }
                    }
                }
            }
      
            if (_tagController.SatisfiesRequirements(effectToApply.EffectData.RequiredTags,effectToApply.EffectData.ForbiddenTags) == false)
            {
                Debug.Log($"Application requirements failed for {effectToApply.EffectData.name}");
                return false;
            }

            if (effectToApply is PersistentEffect persistentEffectToApply)
            {
                if (!_tagController.SatisfiesRequirements(persistentEffectToApply.EffectData.PersistMustBePresentTags,
                    persistentEffectToApply.EffectData.PersistMustBeAbsentTags))
                {
                    Debug.Log($"Failed ongoing requirements for {effectToApply.EffectData.name}");
                    return false;
                }
            }

            bool isAdded = true;
            if (effectToApply is StackableEffect stackableEffect)
            {
                StackableEffect existingStackableEffect = _activeEffects.Find(activeEffect => activeEffect.EffectData == effectToApply.EffectData) as StackableEffect;

                if (existingStackableEffect != null)
                {
                    isAdded = false;

                    if (existingStackableEffect.StackCount == existingStackableEffect.EffectData.StackLimitCount)
                    {
                        foreach (EffectData effectDefinition in existingStackableEffect.EffectData.OverflowEffects)
                        {
                            EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                                .OfType<EffectTypeAttribute>().FirstOrDefault();
                            AbilityEffect overflowEffect = Activator.CreateInstance(attribute.Type, effectDefinition, existingStackableEffect, gameObject) as AbilityEffect;
                            ApplyGameplayEffectToSelf(overflowEffect);
                        }

                        if (existingStackableEffect.EffectData.ClearStackOnOverflow)
                        {
                            RemoveActiveGameplayEffect(existingStackableEffect, true);
                            isAdded = true;
                        }

                        if (existingStackableEffect.EffectData.DenyOverflowApplication)
                        {
                            Debug.Log("Denied overflow application!");
                            return false;
                        }
                    }

                    if (isAdded == false)
                    {
                        existingStackableEffect.StackCount =
                            Math.Min(existingStackableEffect.StackCount + stackableEffect.StackCount,
                                existingStackableEffect.EffectData.StackLimitCount);

                        if (existingStackableEffect.EffectData.StackDurationRefreshPolicy ==
                            GameplayEffectStackingDurationPolicy.RefreshOnSuccessfulApplication)
                        {
                            existingStackableEffect.RemainingDuration = existingStackableEffect.Duration;
                        }

                        if (existingStackableEffect.EffectData.StackPeriodResetPolicy ==
                            GameplayEffectStackingPeriodPolicy.ResetOnSuccessfulApplication)
                        {
                            existingStackableEffect.RemainingPeriod = existingStackableEffect.EffectData.Period;
                        }
                    }
                }
            }

            foreach (EffectData conditionalEffectDefinition in effectToApply.EffectData.ConditionalEffects)
            {
                EffectTypeAttribute attribute = conditionalEffectDefinition.GetType().GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>()
                    .FirstOrDefault();
                AbilityEffect conditionalEffect = Activator.CreateInstance(attribute.Type, conditionalEffectDefinition, effectToApply, effectToApply.Attacker) as AbilityEffect;
                ApplyGameplayEffectToSelf(conditionalEffect);
            }

            List<PersistentEffect> effectsToRemove = new List<PersistentEffect>();

            foreach (PersistentEffect activeEffect in _activeEffects)
            {
                foreach (string tag in activeEffect.EffectData.Tags)
                {
                    if (effectToApply.EffectData.RemoveEffectsWithTags.Contains(tag))
                    {
                        effectsToRemove.Add(activeEffect);
                    }
                }
            }

            foreach (PersistentEffect effectToRemove in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effectToRemove, true);
            }

            if (effectToApply is PersistentEffect persistentEffect)
            {
                if (isAdded)
                    AddGameplayEffect(persistentEffect);
            }
            else
            {
                ExecuteGameplayEffect(effectToApply);
            }

            if (effectToApply.EffectData.SpecialEffectDefinition != null)
                PlaySpecialEffect(effectToApply);

            return true;
        }

        private void AddGameplayEffect(PersistentEffect effect)
        {
            _activeEffects.Add(effect);
            CheckOngoingTagRequirements(effect);

            if (effect.EffectData.IsPeriodic)
            {
                if (effect.EffectData.ExecutePeriodicEffectOnApplication)
                {
                    if (!effect.IsInhibited)
                        ExecuteGameplayEffect(effect);
                }
            }

        }

        private void RemoveActiveGameplayEffect(PersistentEffect effect, bool prematureRemoval)
        {
            _activeEffects.Remove(effect);
           
            if (effect.IsInhibited == false)
            {
                RemoveUninhibitedEffects(effect);
            }
        }

        private void RemoveUninhibitedEffects(PersistentEffect effect)
        {
            foreach (var modifierDefinition in effect.EffectData.ModifierDefinitions)
            {
                if (_statController.Stats.TryGetValue(modifierDefinition.StatName, out Stat stat))
                {
                    stat.RemoveModifierFromSource(effect);
                }
            }

            foreach (string tag in effect.EffectData.GrantedTags)
            {
                _tagController.RemoveTag(tag);
            }

            if (effect.EffectData.SpecialPersistentEffectDefinition != null)
                StopSpecialEffect(effect);

        }
        private void AddUninhibitedEffects(PersistentEffect effect)
        {
            for (int i = 0; i < effect.StatModifiers.Count; i++)
            {
                if (_statController.Stats.TryGetValue(effect.EffectData.ModifierDefinitions[i].StatName, out Stat stat))
                {
                    stat.AddModifier(effect.StatModifiers[i]);
                }
            }

            foreach (string tag in effect.EffectData.GrantedTags)
            {
                _tagController.AddTag(tag);
            }

            if (effect.EffectData.SpecialPersistentEffectDefinition != null)
                PlaySpecialEffect(effect);
        }
        private void ExecuteGameplayEffect(AbilityEffect effect)
        {
            for (int i = 0; i < effect.StatModifiers.Count; i++)
            {
                if (_statController.Stats.TryGetValue(effect.EffectData.ModifierDefinitions[i].StatName,
                    out Stat stat))
                {
                    if (stat is Attribute attribute)
                    {
                        attribute.ApplyModifier(effect.StatModifiers[i]);
                    }
                }
            }
        }

        private void HandleDuration()
        {
            List<PersistentEffect> effectsToRemove = new List<PersistentEffect>();
            foreach (PersistentEffect activeEffect in _activeEffects)
            {
                if (activeEffect.EffectData.IsPeriodic)
                {
                    activeEffect.RemainingPeriod = Math.Max(activeEffect.RemainingPeriod - Time.deltaTime, 0f);

                    if (Mathf.Approximately(activeEffect.RemainingPeriod, 0f))
                    {
                        if (!activeEffect.IsInhibited)
                            ExecuteGameplayEffect(activeEffect);

                        activeEffect.RemainingPeriod = activeEffect.EffectData.Period;
                    }
                }

                if (activeEffect.EffectData.IsInfinite == false)
                {
                    activeEffect.RemainingDuration = Math.Max(activeEffect.RemainingDuration - Time.deltaTime, 0f);
                    
                    if (Mathf.Approximately(activeEffect.RemainingDuration, 0f))
                    {
                        if (activeEffect is StackableEffect stackableEffect)
                        {
                            switch (stackableEffect.EffectData.StackExpirationPolicy)
                            {
                                case GameplayEffectStackingExpirationPolicy.RemoveSingleStackAndRefreshDuration:
                                    stackableEffect.StackCount--;
                                    if (stackableEffect.StackCount == 0)
                                        effectsToRemove.Add(stackableEffect);
                                    else
                                        activeEffect.RemainingDuration = activeEffect.Duration;
                                    break;
                                case GameplayEffectStackingExpirationPolicy.NeverRefresh:
                                    effectsToRemove.Add(stackableEffect);
                                    break;
                            }
                        }
                        else
                        {
                            effectsToRemove.Add(activeEffect);
                        }
                    }
                }
            }
            foreach (PersistentEffect effect in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effect, false);
            }
        }

        public bool CanApplyAttributeModifiers(EffectData effectDefinition)
        {
            foreach (var modifierDefinition in effectDefinition.ModifierDefinitions)
            {
                if (_statController.Stats.TryGetValue(modifierDefinition.StatName, out Stat stat))
                {
                    if (stat is Attribute attribute)
                    {
                        if (modifierDefinition.ModifierType == ModifierOperationType.Additive)
                        {
                            if (attribute.CurrentValue <
                                Mathf.Abs(modifierDefinition.Formula.CalculateValue(gameObject)))
                            {
                                Debug.Log($"{effectDefinition.name} cannot satisfy costs!");
                                return false;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Only addition is supported!");
                            return false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"{modifierDefinition.StatName} is not an attribute!");
                        return false;
                    }
                }
                else
                {
                    Debug.LogWarning($"{modifierDefinition.StatName} not found!");
                    return false;
                }
            }
            return true;
        }

        private void CheckRemovalTagRequirements(string tag)
        {
            _activeEffects.Where(activeEffect => !_tagController.SatisfiesRequirements(
                activeEffect.EffectData.PersistMustBePresentTags, activeEffect.EffectData.PersistMustBeAbsentTags
            )).ToList().ForEach(effect => RemoveActiveGameplayEffect(effect, true));
        }

        private void CheckOngoingTagRequirements(string tag)
        {
            foreach (PersistentEffect activeEffect in _activeEffects)
            {
                CheckOngoingTagRequirements(activeEffect);
            }
        }

        private void CheckOngoingTagRequirements(PersistentEffect effect)
        {
            bool shouldBeInhibited = !_tagController.SatisfiesRequirements(
                effect.EffectData.UninhibitedMustBePresentTags, effect.EffectData.UninhibitedMustBeAbsentTags);

            if (effect.IsInhibited != shouldBeInhibited)
            {
                effect.IsInhibited = shouldBeInhibited;

                if (effect.IsInhibited)
                {
                    RemoveUninhibitedEffects(effect);
                }
                else
                {
                    if (effect.EffectData.IsPeriodic)
                    {
                        switch (effect.EffectData.PeriodicInhibitionPolicy)
                        {
                            case GameplayEffectPeriodInhibitionRemovedPolicy.ResetPeriod:
                                effect.RemainingPeriod = effect.EffectData.Period;
                                break;
                            case GameplayEffectPeriodInhibitionRemovedPolicy.ExecuteAndResetPeriod:
                                ExecuteGameplayEffect(effect);
                                effect.RemainingPeriod = effect.EffectData.Period;
                                break;
                            case GameplayEffectPeriodInhibitionRemovedPolicy.NeverReset:
                                break;
                        }
                    }

                    AddUninhibitedEffects(effect);
                }
            }
        }
    }
}