using BaseProject;

namespace ComboSystemComposition
{
    public class Being : IBeing
    {
        public string Id { get; }
        public TargetType TargetType { get; }
        private double _damage;
        private ModifierController ModifierController { get; }

        public Being(string id, double damage, TargetType targetType)
        {
            Id = id;
            TargetType = targetType;
            _damage = damage;
            ModifierController = new ModifierController(this);
        }

        public void DealDamage(double damage)
        {

        }

        public void ChangeStat(double health)
        {
        }

        public void Update(float deltaTime)
        {
            ModifierController.Update(deltaTime);
        }

        public void Attack(Being target)
        {
            ApplyModifiers(target);
            target.DealDamage(_damage);
            //AttackEvent?.Invoke(this, (Being)target);
            //if(target.IsDead)
            //    KillEvent?.Invoke(this, (Being)target);
        }

        protected void OnDeath()
        {
            //Log.Verbose("OnDeath, "+DeathEvent?.GetInvocationList().Length, "modifiers");
            //DeathEvent?.Invoke(this);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        public void ApplyModifiers(Being target)
        {
            var modifierAppliers = ModifierController.GetModifierAppliers();
            if (modifierAppliers == null)
            {
                Log.Verbose(Id+" has no applier modifiers", "modifiers");
                return;
            }

            foreach (var modifierApplier in modifierAppliers)
                modifierApplier.ApplyModifierToTarget(target);
        }

        public void AddModifier(Modifier modifier)
        {
            ModifierController.TryAddModifier(modifier);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return ModifierController.ContainsModifier(modifier);
        }
    }
}