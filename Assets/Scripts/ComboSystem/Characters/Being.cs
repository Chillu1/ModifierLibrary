namespace ComboSystem
{
    /// <summary>
    ///     Base class for all beings that can have modifiers
    /// </summary>
    public abstract class Being : BaseProject.Being
    {
        protected ModifierController ModifierController { get; }

        protected Being(ComboBeingProperties properties) : base(properties)
        {
            ModifierController = new ModifierController(this);
            AddModifier(properties.ModifierHolder);
        }

        public override void Attack(BaseProject.Being target)
        {
            ApplyModifiers((Being)target);
            base.Attack(target);
        }

        public virtual void ApplyModifiers(Being target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            ModifierController.ListModifiers(modifierAppliers);
            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.ApplyModifierToTarget(target);
        }

        public void AddModifier(ModifierHolder modifierHolder)
        {
            if (modifierHolder == null)
                return;

            foreach (var modifierParams in modifierHolder.modifiers)
                AddModifier(modifierParams.modifier, modifierParams.addModifierProperties);
        }

        public void AddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default)
        {
            ModifierController.TryAddModifier(modifier, parameters);
        }

        public virtual bool IsValidTarget(Modifier modifier)
        {
            return true;
        }

        public override string ToString()
        {
            return Id + ModifierController;
        }
    }
}