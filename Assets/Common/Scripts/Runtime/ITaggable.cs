using System.Collections.ObjectModel;

namespace Common.Runtime
{
    public interface ITaggable
    {
        ReadOnlyCollection<string> Tags { get; }
    }
}