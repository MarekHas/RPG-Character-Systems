using LevelUpSystem;
using StatsSystem;
using UnityEngine;
using UnityEngine.AI;
using AbilitySystem;
using Game;
using AbilitySystem.UI;

namespace MyGame.Scripts
{
    [RequireComponent(typeof(CharacterStatsController))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AbilityController))]
    public class Player : CombatableCharacter
    {
        private ICanLevelUp _canLevelUp;
        [SerializeField] private AbilitiesUI _abilitiesUI;
        [SerializeField] private GameObject _target;
        private NavMeshAgent _navMeshAgent;
        private AbilityController _abilityController;

        protected override void Awake()
        {
            base.Awake();

            _canLevelUp = GetComponent<ICanLevelUp>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _abilityController = GetComponent<AbilityController>();
        }

        private void Update()
        {
          
            if (Input.GetKeyDown(KeyCode.G))
            {
                _abilityController.TryActivateAbility("Shout", _target);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                _abilityController.TryActivateAbility("Heal", _target);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _canLevelUp.CurrentExperience += 100;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                _abilitiesUI.Show();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _abilitiesUI.Hide();
            }
        }
    }
}