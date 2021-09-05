namespace ComboSystem
{
    /// <summary>
    ///     Base class for all beings that can have modifiers
    /// </summary>
    public abstract class Being : BaseProject.Being
    {
        public ModifierController ModifierController { get; }

        protected Being(string id, float maxHealth, float damage) : base(id, maxHealth, damage)
        {
            ModifierController = new ModifierController(this);
        }

        public override void Attack(BaseProject.Being target)
        {
            ApplyModifiers((Being)target);
            base.Attack(target);
        }

        public virtual void ApplyModifiers(Being target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.ApplyModifierToTarget(target);
        }
        public virtual void AddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default)
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