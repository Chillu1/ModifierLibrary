using BaseProject;

namespace ModifierSystem
{
    public interface ITimeComponent : IStatusTagsHolder
    {
        bool IsRemove { get; }
        void Update(float deltaTime, double statusResistance = 1d);
    }
}