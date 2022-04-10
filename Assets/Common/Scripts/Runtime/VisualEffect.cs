using System;
using System.Collections;
using UnityEngine;

namespace Common.Runtime
{
    public class VisualEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Coroutine _coroutine;
        public event Action<VisualEffect> Finished;
        public bool IsLooping => _particleSystem.main.loop;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            _particleSystem.Play();
            
            if (_particleSystem.main.loop == false)
            {
                _coroutine = StartCoroutine(WaitForDuration());
            }
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _particleSystem.Stop();
            Finished?.Invoke(this);
        }

        private IEnumerator WaitForDuration()
        {
            yield return new WaitForSeconds(_particleSystem.main.duration);

            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            
            yield return new WaitUntil(() => _particleSystem.particleCount == 0);
            
            Finished?.Invoke(this);
        }
    }
}