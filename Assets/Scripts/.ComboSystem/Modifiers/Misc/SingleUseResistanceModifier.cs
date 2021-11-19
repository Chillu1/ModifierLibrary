using BaseProject;

namespace ComboSystem
{
    public class SingleUseResistanceModifier : SingleUseModifier<ResistanceModifierData>
    {
        public SingleUseResistanceModifier(string id, ResistanceModifierData data, ModifierProperties modifierProperties = default) : base(id, data, modifierProperties)
        {
        }

        protected override void Effect()
        {
            if(Data.Value != 0d)
                Target!.Resistances.ChangeValue(Data.DamageType, Data.Value);
            if (Data.Multiplier != 0d)
                Target!.Resistances.ChangeMultiplier(Data.DamageType, Data.Multiplier);
            Log.Info(Target.Resistances);
            base.Effect();
        }

        protected override void Remove()
        {
            if(Data.Value != 0d)
                Target!.Resistances.ChangeValue(Data.DamageType, -Data.Value);
            if(Data.Multiplier != 0d)
                Target!.Resistances.ChangeMultiplier(Data.DamageType, -Data.Multiplier);
            IsOn = false;
            Log.Info("Remove");
            //base.Remove();//Commented so the modifier doesn't actually get removed
        }
    }
}