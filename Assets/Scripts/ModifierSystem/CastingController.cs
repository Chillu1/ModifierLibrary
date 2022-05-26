using BaseProject;

namespace ModifierSystem
{
    public class CastingController
    {
        private readonly ModifierController _modifierController;
        private readonly StatusEffects _statusEffects;

        public CastingController(ModifierController modifierController, StatusEffects statusEffects)
        {
            _modifierController = modifierController;
            _statusEffects = statusEffects;
        }


        public void Update(float deltaTime)
        {
        }

        public bool CastModifier(Being target, string modifierId)
        {
            if (!_modifierController.ContainsModifier(modifierId, out var modifier))
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

            if (!_statusEffects.LegalActions.HasFlag(LegalAction.Cast)) //Can't cast
                return false;

            modifier.TryApply(target);

            return true;
        }
    }
}