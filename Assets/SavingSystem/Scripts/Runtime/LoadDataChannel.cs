using System;
using UnityEngine;

namespace SavingSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "LoadDataChannel", menuName = "SavingSystem/LoadDataChannel", order = 0)]
    public class LoadDataChannel : ScriptableObject
    {
        public event Action Load;

        public void LoadData()
        {
            Load?.Invoke();
        }
    }
}