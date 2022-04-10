using System.Collections.Generic;
using System.Linq;
using Common.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public partial class GameplayEffectController
    {
        private List<VisualEffect> _statusEffects = new List<VisualEffect>();
        private Dictionary<SpecialEffectData, int> _specialEffectCountMap = new Dictionary<SpecialEffectData, int>();
        private Dictionary<SpecialEffectData, VisualEffect> _specialEffectMap = new Dictionary<SpecialEffectData, VisualEffect>();
        private float _timePeriod = 1f;
        private int _index;
        private float _leftTime;

        private void HandleStatusEffects()
        {
            if (_statusEffects.Count > 1)
            {
                _leftTime = Mathf.Max(_leftTime - Time.deltaTime, 0f);

                if (Mathf.Approximately(_leftTime, 0f))
                {
                    _statusEffects[_index].gameObject.SetActive(false);
                    _index = (_index + 1) % _statusEffects.Count;
                    _statusEffects[_index].gameObject.SetActive(true);
                    _leftTime = _timePeriod;
                }
            }
        }

        private void PlaySpecialEffect(PersistentEffect effect)
        {
            VisualEffect visualEffect = Instantiate(effect.EffectData.SpecialPersistentEffectDefinition.Prefab, transform);
            visualEffect.Finished += visualEffect => Destroy(visualEffect.gameObject);

            if (effect.EffectData.SpecialPersistentEffectDefinition.EffectPosition == EffectPosition.Center)
            {
                visualEffect.transform.localPosition = Utils.GetCenterOfCollider(transform);
            }
            else if (effect.EffectData.SpecialPersistentEffectDefinition.EffectPosition == EffectPosition.Above)
            {
                visualEffect.transform.localPosition = Utils.GetComponentHeight(gameObject) * Vector3.up;
            }

            if (visualEffect.IsLooping)
            {
                if (_specialEffectCountMap.ContainsKey(effect.EffectData.SpecialPersistentEffectDefinition))
                {
                    _specialEffectCountMap[effect.EffectData.SpecialPersistentEffectDefinition]++;
                }
                else
                {
                    _specialEffectCountMap.Add(effect.EffectData.SpecialPersistentEffectDefinition, 1);
                    _specialEffectMap.Add(effect.EffectData.SpecialPersistentEffectDefinition, visualEffect);

                    if (effect.EffectData.Tags.Any(tag => tag.StartsWith("status")))
                    {
                        _statusEffects.Add(visualEffect);
                    }
                }
            }

            visualEffect.Play();
        }

        private void PlaySpecialEffect(AbilityEffect effect)
        {
            VisualEffect visualEffect = Instantiate(effect.EffectData.SpecialEffectDefinition.Prefab,
                transform.position, transform.rotation);
            visualEffect.Finished += visualEffect => Destroy(visualEffect.gameObject);

            if (effect.EffectData.SpecialEffectDefinition.EffectPosition == EffectPosition.Center)
            {
                visualEffect.transform.position += Utils.GetCenterOfCollider(transform);
            }
            else if (effect.EffectData.SpecialEffectDefinition.EffectPosition == EffectPosition.Above)
            {
                visualEffect.transform.position += Utils.GetComponentHeight(gameObject) * Vector3.up;
            }
            visualEffect.Play();
        }

        private void StopSpecialEffect(PersistentEffect effect)
        {
            if (_specialEffectCountMap.ContainsKey(effect.EffectData.SpecialPersistentEffectDefinition))
            {
                _specialEffectCountMap[effect.EffectData.SpecialPersistentEffectDefinition]--;
                if (_specialEffectCountMap[effect.EffectData.SpecialPersistentEffectDefinition] == 0)
                {
                    _specialEffectCountMap.Remove(effect.EffectData.SpecialPersistentEffectDefinition);
                    VisualEffect visualEffect = _specialEffectMap[effect.EffectData.SpecialPersistentEffectDefinition];
                    visualEffect.Stop();
                    _specialEffectMap.Remove(effect.EffectData.SpecialPersistentEffectDefinition);

                    if (effect.EffectData.Tags.Any(tag => tag.StartsWith("status")))
                    {
                        _statusEffects.Remove(visualEffect);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Trying to remove a status effect that does not exist!");
            }
        }
    }
}