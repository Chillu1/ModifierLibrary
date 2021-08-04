namespace ComboSystem
{
    //Design:
    //Clean interaction between applied buffs on unit
    //All buffs in a single collection, ticking
    //Calculate base stats, then add percentages, every time our stats change, recalculate
    public class ComboSystemClass //RENAME
    {

    }

    public abstract class BaseModifier
    {
        public float Duration { get; protected set; }

        public abstract void Apply();

        public virtual void Tick()
        {
        }
    }

    public sealed class SpeedModifier : BaseModifier
    {
        public float value;
        private ICharacter target;

        public SpeedModifier(float value)
        {
            this.value = value;
        }

        public override void Apply()
        {
            //target.MovementSpeed -= value;
        }
    }

    public interface ICharacter
    {
        public float MovementSpeed { get; }

        public void ApplyMovementSpeed();
    }

    public class Player : ICharacter
    {
        public float MovementSpeed { get; private set; }
        public void ApplyMovementSpeed()
        {
            //Movement speed calc
        }
    }

}