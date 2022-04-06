using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Runtime;
using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    public class Sword : MeleeWeapon, ITaggable
    {
        [SerializeField] private List<string> _tags = new List<string>() { "physical" };

        public ReadOnlyCollection<string> Tags => _tags.AsReadOnly();
    }
}