using Common.Nodes;
using UnityEngine;

namespace LevelUpSystem.Nodes
{
    public class LevelNode : FunctionNode
    {
        public ICanLevelUp CanLevelUP;
        public override float Value => CanLevelUP.Level;

        public override float CalculateValue(GameObject source)
        {
            ICanLevelUp canLevelUP = source.GetComponent<ICanLevelUp>();
            return canLevelUP.Level;
        }
    }
}