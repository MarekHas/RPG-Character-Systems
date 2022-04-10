using Common.Nodes;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityLevelNode : FunctionNode
    {
        public override float Value => Ability.Level;
        public Ability Ability;
        
        [SerializeField] private string _name;
        
        public string Name => _name;
        
        public override float CalculateValue(GameObject source)
        {
            AbilityController abilityController = source.GetComponent<AbilityController>();
            return abilityController.Abilities[_name].Level;
        }
    }
}