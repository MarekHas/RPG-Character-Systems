using System;
using System.Collections.Generic;
using UnityEngine;

namespace SavingSystem.Scripts.Runtime
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private SaveData _saveData;
        [SerializeField] private SaveDataChannel _saveDataChannel;
        [SerializeField] private LoadDataChannel _loadDataChannel;
        [HideInInspector, SerializeField] private string _id;

        private void OnEnable()
        {
            _loadDataChannel.Load += OnLoadData;
            _saveDataChannel.Save += OnSaveData;
        }

        private void OnDisable()
        {
            _loadDataChannel.Load -= OnLoadData;
            _saveDataChannel.Save -= OnSaveData;
        }

        private void OnSaveData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                data[savable.GetType().ToString()] = savable.Data;
            }
            _saveData.Save(_id, data);
        }

        private void OnLoadData()
        {
            _saveData.Load(_id, out object data);
            Dictionary<string, object> dictionary = data as Dictionary<string, object>;

            foreach (ISavable savable in GetComponents<ISavable>())
            {
                savable.Load(dictionary[savable.GetType().ToString()]);
            }
        }

        private void Reset()
        {
            _id = Guid.NewGuid().ToString();
        }
    }
}