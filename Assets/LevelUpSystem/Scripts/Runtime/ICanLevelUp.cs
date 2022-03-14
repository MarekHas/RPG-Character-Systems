using System;

namespace LevelUpSystem
{
    public interface ICanLevelUp
    {
        int Level { get; }
        event Action LevelChanged;
        event Action ExperienceChanged;
        int CurrentExperience { get; set; }
        int RequiredExperience { get; }
        bool IsInitialized { get; }
        event Action Initialized;
        event Action WillUninitialize;
    }
}
