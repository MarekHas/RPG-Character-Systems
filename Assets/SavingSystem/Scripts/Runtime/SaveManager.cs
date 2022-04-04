using UnityEngine;

namespace SavingSystem.Scripts.Runtime
{
    [DefaultExecutionOrder(1)]
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveData _saveData;

        private void Awake()
        {
            if (_saveData.PreviousSaveExists)
                _saveData.Load();
        }

        private void OnApplicationQuit()
        {
            _saveData.Save();
        }
    }
}