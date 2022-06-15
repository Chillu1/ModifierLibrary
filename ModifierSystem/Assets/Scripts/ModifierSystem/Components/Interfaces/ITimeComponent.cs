using BaseProject;

namespace ModifierSystem
{
    public interface ITimeComponent : IStatusTagsHolder, IDisplay
    {
        bool IsRemove { get; }
        void Update(float deltaTime, double statusResistance = 1d);
    }
}