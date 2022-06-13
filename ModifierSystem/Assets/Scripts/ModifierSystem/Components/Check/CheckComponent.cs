using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for Cost, Cooldown, and Chance.
    /// </summary>
    public class CheckComponent : ICheckComponent
    {
        private IEffectComponent[] EffectComponents { get; }

        [CanBeNull] public ICooldownComponent CooldownComponent { get; }
        [CanBeNull] public ICostComponent CostComponent { get; }
        [CanBeNull] public IChanceComponent ChanceComponent { get; }

        public CheckComponent(IEffectComponent effectComponent, ICooldownComponent cooldownComponent = null, ICostComponent costComponent = null,
            IChanceComponent chanceComponent = null)
        {
            EffectComponents = new[] { effectComponent };
            CooldownComponent = cooldownComponent;
            CostComponent = costComponent;
            ChanceComponent = chanceComponent;
        }
        public CheckComponent(IEffectComponent[] effectComponents, ICooldownComponent cooldownComponent = null, ICostComponent costComponent = null,
            IChanceComponent chanceComponent = null)
        {
            EffectComponents = effectComponents;
            CooldownComponent = cooldownComponent;
            CostComponent = costComponent;
            ChanceComponent = chanceComponent;
        }

        public void Effect()
        {
            if (Check())
            {
                foreach (var effectComponent in EffectComponents)
                    effectComponent.SimpleEffect();
            }
        }

        /// <summary>
        ///     Used for checking if effect is valid: Cost, Cooldown, and Chance.
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            bool valid = true;

            if (CooldownComponent != null && !CooldownComponent.IsReady())
            {
                valid = false;
                //Log.Error("Can't apply " + Id + " because it's on cooldown", "modifiers");
            }
            if (CostComponent != null && !CostComponent.ContainsCost())
            {
                valid = false;
                //Log.Error("Can't apply " + Id + " because it costs more than the owner has", "modifiers");
            }
            if (ChanceComponent != null && !ChanceComponent.Roll())
            {
                valid = false;
                //Log.Info("Can't apply " + Id + " because it failed the chance check", "modifiers");
            }

            return valid;
        }

        public void Apply()
        {
            CostComponent?.ApplyCost();
            CooldownComponent?.ResetTimer();
        }

        public bool EffectComponentIsOfType<T>() where T : IEffectComponent
        {
            foreach (var effectComponent in EffectComponents)
            {
                if (effectComponent.GetType() == typeof(T))
                    return true;
            }

            return false;
        }
    }
}