using System;
using BaseProject;
using BaseProject.Utils;

namespace ComboSystem
{
    /// <summary>
    ///     Base class for all beings that can have modifiers
    /// </summary>
    public abstract class Being : BaseProject.Being, IEventCopy<Being>
    {
        //Owner, target
        public event Action<Being, Being> AttackEvent;
        public event Action<Being, Being> KillEvent;
        public event Action<Being, Being> CastEvent;
        public event Action<Being> DeathEvent;

        /// <summary>
        ///     On getting a combo
        /// </summary>
        public event Action<Being> ComboEvent;
        protected ModifierController ModifierController { get; }

        protected Being(ComboBeingProperties properties) : base(properties)
        {
            ModifierController = new ModifierController(this);
            AddModifier(properties.ModifierHolder);
        }

        public virtual void Update(float deltaTime)
        {
            ModifierController.Update(deltaTime);
        }

        public override void Attack(BaseProject.Being target)
        {
            ApplyModifiers((Being)target);
            base.Attack(target);
            AttackEvent?.Invoke(this, (Being)target);
            if(target.IsDead)
                KillEvent?.Invoke(this, (Being)target);
        }

        protected override void OnDeath()
        {
            //Log.Verbose("OnDeath, "+DeathEvent?.GetInvocationList().Length, "modifiers");
            DeathEvent?.Invoke(this);
        }

        /// <summary>
        ///     Apply modifier appliers to target
        /// </summary>
        public virtual void ApplyModifiers(Being target)
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

        public void AddModifier(ModifierHolder modifierHolder)
        {
            if (modifierHolder == null)
            {
                Log.Verbose("ModifierHolder is null on: "+ Id, "modifiers");
                return;
            }

            foreach (var modifierParams in modifierHolder.modifiers)
            {
                AddModifier(modifierParams.modifier, modifierParams.addModifierProperties);
            }
            //ModifierController.ListModifiers();
        }

        public void AddModifier(Modifier modifier, AddModifierParameters parameters = AddModifierParameters.Default, ActivationCondition condition = default)
        {
            if (typeof(ModifierApplier<ModifierApplierData>).IsSameOrSubclass(modifier.GetType()))
            {
                //A special applier modifier
                if (!parameters.HasFlag(AddModifierParameters.NullStartTarget) || parameters == AddModifierParameters.Default)//Wrong? Can owner be target in applier mods?
                {
                    Log.Error("Wrong parameters for adding a modifier applier: "+parameters.ToString(), "modifiers");
                    ModifierController.TryAddModifier(modifier, AddModifierParameters.DefaultOffensive, condition);
                    return;
                }
            }

            ModifierController.TryAddModifier(modifier, parameters, condition);
        }

        public void AddModifierApplier(ModifierApplier<ModifierApplierData> modifier,
            AddModifierParameters parameters = AddModifierParameters.DefaultOffensive, ActivationCondition condition = default)
        {
            AddModifier(modifier, parameters, condition);
        }

        public bool ContainsModifier(Modifier modifier)
        {
            return ModifierController.ContainsModifier(modifier);
        }

        public void ListModifiers()
        {
            ModifierController.ListModifiers();
        }

        public virtual bool IsValidTarget(Modifier modifier)
        {
            return true;
        }

        public virtual void CopyEvents(Being prototype)
        {
            AttackEvent = prototype.AttackEvent;
            KillEvent = prototype.KillEvent;
            CastEvent = prototype.CastEvent;
            DeathEvent = prototype.DeathEvent;
            ComboEvent = prototype.ComboEvent;
            //Copy modifierEvents
            //problem, we copy the event, but the target is wrong modifierController (old)
            ModifierController.CopyEvents(prototype.ModifierController);
        }

        public override string ToString()
        {
            return Id + ModifierController;
        }
    }
}