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

            bool multipleEffectTypes = effectComponents.Length > 0 && effectTimeComponents is { Length: > 0 };
            
            string text = "";
            int i = 0;
            foreach (var effectComponent in EffectComponents.EmptyIfNull())
            {
                if (multipleEffectTypes && i == 0)
                    text += "Initial: ";
                text += effectComponent.Info;
                i++;
            }
            
            i = 0;
            foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
            {
                if (multipleEffectTypes && i == 0)
                    text += "Time: ";
                text += effectComponent.Info;
                i++;
            }

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

        public string GetBasicInfo()
        {
            string text = "";
            //foreach (var effectComponent in EffectComponents.EmptyIfNull())
            //    text += effectComponent.Info;
            //foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
            //    text += effectComponent.Info;

            text += CostComponent?.GetBasicInfo();
            text += CooldownComponent?.GetBasicInfo();
            text += ChanceComponent?.GetBasicInfo();

            return text;
        }
        
        public string GetBattleInfo()
        {
            string text = "";
            //foreach (var effectComponent in EffectComponents.EmptyIfNull())
            //    text += effectComponent.Info;
            //foreach (var effectComponent in TimeEffectComponents.EmptyIfNull())
            //    text += effectComponent.Info;

            text += CostComponent?.GetBattleInfo();
            text += CooldownComponent?.GetBattleInfo();
            text += ChanceComponent?.GetBattleInfo();

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