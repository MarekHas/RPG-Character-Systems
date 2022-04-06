using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;

        private void Reset()
        {
            _id = gameObject.name;
        }
    }
}