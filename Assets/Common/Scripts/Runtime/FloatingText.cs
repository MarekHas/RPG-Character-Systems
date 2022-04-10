using System;
using DG.Tweening;
using UnityEngine;

namespace Common.Runtime
{
    public class FloatingText : MonoBehaviour
    {
        public event Action<FloatingText> Finished;
        public float Time = 1.5f;
        private Transform _mainCamera;
        private TextMesh _textMesh;

        private void Awake()
        {
            _textMesh = GetComponent<TextMesh>();
            _mainCamera = Camera.main.transform;
        }

        public void Animate()
        {
            transform.DOMove(transform.position + Vector3.up, Time).OnKill(() => Finished?.Invoke(this));
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _mainCamera.forward);
        }

        public void Set(string value, Color color)
        {
            _textMesh.text = value;
            _textMesh.color = color;
        }
    }
}