namespace SavingSystem.Scripts.Runtime
{
    public interface ISavable
    {
        object Data { get; }
        void Load(object data);
    }
}