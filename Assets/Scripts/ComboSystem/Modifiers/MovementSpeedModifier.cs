using System;

namespace ComboSystem
{
    public class MovementSpeedModifier : SingleUseModifier<MovementSpeedModifierData>//, IEquatable<MovementSpeedModifier>
    {
        public MovementSpeedModifier(string id, MovementSpeedModifierData speedBuffPlayerData, ModifierProperties properties = default) : base(id, properties)
        {
            Data = speedBuffPlayerData;
        }

        protected override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target!.ChangeStat(StatType.MovementSpeed, Data.MovementSpeed);
            return true;
        }

        protected override void Remove()
        {
            Target.ChangeStat(StatType.MovementSpeed, -Data.MovementSpeed);
            base.Remove();
        }

        // public bool Equals(MovementSpeedModifier other)
        // {
        //     //if (!base.Equals(other))
        //     //    return false;
        //     return Data == other.Data && this.GetType() == other.GetType() && Math.Abs(Data.MovementSpeed - other.Data.MovementSpeed) < 0.0001f;
        // }
    }
}