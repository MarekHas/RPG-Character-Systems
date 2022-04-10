using StatsSystem;
using UnityEngine;
using UnityEngine.AI;
using AbilitySystem;

namespace Game
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(StatsController))]
    [RequireComponent(typeof(AbilityController))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private float _baseSpeed = 3.5f;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private StatsController _statController;
        private AbilityController _abilityController;
        private static readonly int MOVEMENT_SPEED = Animator.StringToHash("MovementSpeed");
        private static readonly int VELOCITY = Animator.StringToHash("Velocity");
        private static readonly int ATTACK_SPEED = Animator.StringToHash("AttackSpeed");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _statController = GetComponent<StatsController>();
            _abilityController = GetComponent<AbilityController>();
        }

        private void Update()
        {
            _animator.SetFloat(VELOCITY, _navMeshAgent.velocity.magnitude / _navMeshAgent.speed);
        }

        private void OnEnable()
        {
            _statController.Initialized += OnStatControllerInitialized;

            if (_statController.IsInitialized)
            {
                OnStatControllerInitialized();
            }

            _abilityController.ActivatedAbility += ActivateAbility;
        }

        private void OnDisable()
        {
            _statController.Initialized -= OnStatControllerInitialized;
            if (_statController.IsInitialized)
            {
                _statController.Stats["MovementSpeed"].ValueChanged -= OnMovementSpeedChanged;
                _statController.Stats["AttackSpeed"].ValueChanged -= OnAttackSpeedChanged;
            }
        }

        #region Animation Events

        public void Shoot()
        {
            if (_abilityController.CurrentAbility is ProjectileAbility projectileAbility)
            {
                projectileAbility.Shoot(_abilityController.Target);
            }
        }

        public void Cast()
        {
            if (_abilityController.CurrentAbility is SingleTargetAbility singleTargetAbility)
            {
                singleTargetAbility.Cast(_abilityController.Target);
            }
        }

        #endregion

        private void ActivateAbility(ActiveAbility activeAbility)
        {
            _animator.SetTrigger(activeAbility.AbilityDescription.AnimationName);
        }

        private void OnStatControllerInitialized()
        {
            OnMovementSpeedChanged();
            OnAttackSpeedChanged();

            _statController.Stats["MovementSpeed"].ValueChanged += OnMovementSpeedChanged;
            _statController.Stats["AttackSpeed"].ValueChanged += OnAttackSpeedChanged;
        }

        private void OnMovementSpeedChanged()
        {
            _animator.SetFloat(MOVEMENT_SPEED, _statController.Stats["MovementSpeed"].Value / 100f);
            _navMeshAgent.speed = _baseSpeed * _statController.Stats["MovementSpeed"].Value / 100f;
        }

        private void OnAttackSpeedChanged()
        {
            _animator.SetFloat(ATTACK_SPEED, _statController.Stats["AttackSpeed"].Value / 100f);
        }
    }
}