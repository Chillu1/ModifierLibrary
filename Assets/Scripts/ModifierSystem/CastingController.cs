using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for casting modifiers on targets
    /// </summary>
    public class CastingController
    {
        private ModifierController ModifierController { get; }
        private StatusEffects StatusEffects { get; }
        private TargetingSystem TargetingSystem { get; }

        public CastingController(ModifierController modifierController, StatusEffects statusEffects, TargetingSystem targetingSystem)
        {
            ModifierController = modifierController;
            StatusEffects = statusEffects;
            TargetingSystem = targetingSystem;
        }


        public void Update(float deltaTime)
        {
            //foreach (var castingModifier in _castingModifiers)
            {
                //if mana?
                //if cooldown

                //try cast
            }
        }

        public bool CastModifier(string modifierId)
        {
            return CastModifier((Being)TargetingSystem.CastTarget, modifierId);
        }

        public bool CastModifier(Being target, string modifierId)
        {
            if (!ModifierController.ContainsModifier(modifierId, out var modifier))
            {
                Log.Error("Modifier " + modifierId + " not present in collection", "modifiers");
                return false;
            }

            if (!modifier.IsApplierModifier)
            {
                //TODO Not sure, about this one, but probably true
                Log.Error("Can't cast a non-applier modifier: " + modifierId, "modifiers");
                return false;
            }

            if (!modifier.ApplierType.HasFlag(ApplierType.Cast))
            {
                Log.Error("Can't cast a non-cast applier modifier: " + modifierId, "modifiers");
                return false;
            }

            if (modifier.ApplierType == ApplierType.Attack)
            {
                Log.Error("Can't cast an attack modifier: " + modifierId, "modifiers");
                return false;
            }

            if (!StatusEffects.LegalActions.HasFlag(LegalAction.Cast)) //Can't cast
                return false;

            modifier.TryCast(target);
            //modifier.TryApply(target);

            return true;
        }
    }
}