using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Common.Runtime
{
    public class TagController : MonoBehaviour, ITaggable
    {
        private Dictionary<string, int> _tagCountMap = new Dictionary<string, int>();
        public event Action<string> TagAdded;
        public event Action<string> TagRemoved;
        public ReadOnlyCollection<string> Tags => _tagCountMap.Keys.ToList().AsReadOnly();

        public bool Contains(string tag)
        {
            return _tagCountMap.ContainsKey(tag);
        }

        public bool ContainsAny(IEnumerable<string> tags)
        {
            return tags.Any(_tagCountMap.ContainsKey);
        }

        public bool ContainsAll(IEnumerable<string> tags)
        {
            return tags.All(_tagCountMap.ContainsKey);
        }

        public bool SatisfiesRequirements(IEnumerable<string> mustBePresentTags, IEnumerable<string> mustBeAbsentTags)
        {
            return ContainsAll(mustBePresentTags) && !ContainsAny(mustBeAbsentTags);
        }

        public void AddTag(string tag)
        {
            if (_tagCountMap.ContainsKey(tag))
            {
                _tagCountMap[tag]++;
            }
            else
            {
                _tagCountMap.Add(tag, 1);
                TagAdded?.Invoke(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            if (_tagCountMap.ContainsKey(tag))
            {
                _tagCountMap[tag]--;
                if (_tagCountMap[tag] == 0)
                {
                    _tagCountMap.Remove(tag);
                    TagRemoved?.Invoke(tag);
                }
            }
            else
            {
                Debug.LogWarning("Attempting to remove a tag that does not exist!");
            }
        }
    }
}