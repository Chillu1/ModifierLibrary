using BaseProject;

namespace ComboSystem
{
    public class ResistanceChangeDurationModifier : SingleUseDurationModifier<ResistanceChangeDurationModifierData>
    {
        public ResistanceChangeDurationModifier(string id, ResistanceChangeDurationModifierData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override void Effect()
        {
            if(Data.Value != 0d)
                Target!.Resistances.ChangeValue(Data.DamageType, Data.Value);
            if(Data.Multiplier != 0d)
                Target!.Resistances.ChangeMultiplier(Data.DamageType, Data.Multiplier);
            //Log.Info("Add: " +Target.Resistances);
        }

        protected override void Remove()
        {
            if(Data.Value != 0d)
                Target!.Resistances.ChangeValue(Data.DamageType, -Data.Value);
            if(Data.Multiplier != 0d)
                Target!.Resistances.ChangeMultiplier(Data.DamageType, -Data.Multiplier);
            //Log.Info(Target.Resistances);
            base.Remove();
        }
    }
}