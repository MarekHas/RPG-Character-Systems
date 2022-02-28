using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    public class StatModifier
    {
        public Object Source { get; set; }
        public int Magnitude { get; set; }
        public ModifierOperationType Type { get; set; }
    }
}
