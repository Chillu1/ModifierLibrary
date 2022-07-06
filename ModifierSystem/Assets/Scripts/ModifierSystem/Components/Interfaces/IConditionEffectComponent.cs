using BaseProject;

namespace ModifierSystem
{
    public interface IConditionEffectComponent
    {
        void ConditionEffect(BaseProject.Unit receiver, BaseProject.Unit acter);
    }
}