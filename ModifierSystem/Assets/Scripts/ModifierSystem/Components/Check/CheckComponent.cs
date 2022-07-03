using BaseProject.Utils;
using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for Cost, Cooldown, and Chance.
    /// </summary>
    public class CheckComponent : ICheckComponent
    {
        private IEffectComponent[] EffectComponents { get; }
        [CanBeNull] private IEffectComponent[] TimeEffectComponents { get; }

        [CanBeNull] public ICostComponent CostComponent { get; }
        [CanBeNull] public ICooldownComponent CooldownComponent { get; }
        [CanBeNull] public IChanceComponent ChanceComponent { get; }

        public string Info { get; }

        public CheckComponent(IEffectComponent[] effectComponents, IEffectComponent[] effectTimeComponents = null,
            ICostComponent costComponent = null, ICooldownComponent cooldownComponent = null, IChanceComponent chanceComponent = null)
        {
            EffectComponents = effectComponents;
            TimeEffectComponents = effectTimeComponents;
            CostComponent = costComponent;
            CooldownComponent = cooldownComponent;
            ChanceComponent = chanceComponent;

            string text = "";
            foreach (var effectComponent in EffectComponents.EmptyIfNull())
                text += effectComponent.Info;
            foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
                text += effectComponent.Info;

            Info = text;
        }

        public void Effect()
        {
            if (Check())
            {
                foreach (var effectComponent in EffectComponents.EmptyIfNull())
                    effectComponent.SimpleEffect();
            }
        }

        public void EffectTime()
        {
            if (Check())
            {
                foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
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

        public string DisplayText()
        {
            string text = "";
            //foreach (var effectComponent in EffectComponents.EmptyIfNull())
            //    text += effectComponent.Info;
            //foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
            //    text += effectComponent.Info;

            text += CostComponent?.DisplayText() + CooldownComponent?.DisplayText() + ChanceComponent?.DisplayText();

            return text;
        }

        public bool EffectComponentIsOfType<T>() where T : IEffectComponent
        {
            foreach (var effectComponent in EffectComponents)
            {
                if (effectComponent.GetType() == typeof(T))
                    return true;
            }
            foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
            {
                if (effectComponent.GetType() == typeof(T))
                    return true;
            }

            return false;
        }
    }
}