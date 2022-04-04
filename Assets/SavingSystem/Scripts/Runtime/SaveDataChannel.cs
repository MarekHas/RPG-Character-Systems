using System;
using UnityEngine;

namespace SavingSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "SaveDataChannel", menuName = "SavingSystem/SaveDataChannel", order = 0)]
    public class SaveDataChannel : ScriptableObject
    {
        public event Action Save;

        public void SaveData()
        {
            Save?.Invoke();
        }
    }
}