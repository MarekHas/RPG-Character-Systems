using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SavingSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "SavingSystem/SaveData", order = 0)]
    public class SaveData : ScriptableObject
    {
        [SerializeField] private LoadDataChannel _loadDataChannel;
        [SerializeField] private SaveDataChannel _saveDataChannel;
        [SerializeField] private string _fileName;
        [HideInInspector, SerializeField] private string _path;

        private Dictionary<string, object> _data = new Dictionary<string, object>();
        public bool PreviousSaveExists => File.Exists(_path);

        [ContextMenu("Delete save file")]
        private void DeleteSave()
        {
            if (PreviousSaveExists)
            {
                File.Delete(_path);
            }
        }

        public void Save(string id, object data)
        {
            _data[id] = data;
        }

        public void Load(string id, out object data)
        {
            data = _data[id];
        }

        public void Load()
        {
            FileManager.LoadFromBinaryFile(_path, out _data);

            _loadDataChannel.LoadData();
            _data.Clear();
        }

        public void Save()
        {
            if (PreviousSaveExists)
                FileManager.LoadFromBinaryFile(_path, out _data);

            _saveDataChannel.SaveData();

            FileManager.SaveToBinaryFile(_path, _data);
            _data.Clear();
        }

        private void OnValidate()
        {
            _path = Path.Combine(Application.persistentDataPath, _fileName);
        }
    }
}