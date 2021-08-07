using System;

namespace ComboSystem
{
    public class StatChangeModifier : SingleUseModifier<StatChangeModifierData>//, IEquatable<MovementSpeedModifier>
    {
        public StatChangeModifier(string id, StatChangeModifierData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override bool Apply()
        {
            if (!ApplyIsValid())
                return false;
            Target!.ChangeStat(Data.StatType, Data.Value);
            return true;
        }

        protected override void Remove()
        {
            Target.ChangeStat(Data.StatType, -Data.Value);
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