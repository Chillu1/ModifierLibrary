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

        private readonly List<Modifier> _castModifiers = new List<Modifier>();
        private float _castTimer = AutomaticCastCooldown;
        private const float AutomaticCastCooldown = 0.1f;

        private bool _globalAutomaticCast;

        public CastingController(ModifierController modifierController, StatusEffects statusEffects, TargetingSystem targetingSystem)
        {
            ModifierController = modifierController;
            StatusEffects = statusEffects;
            TargetingSystem = targetingSystem;
        }

        public void Update(float deltaTime)
        {
            _castTimer -= deltaTime;
            if (_castTimer > 0)
                return;

            _castTimer = AutomaticCastCooldown;

            foreach (var castingModifier in _castModifiers)
            {
                if (castingModifier.IsAutomaticCasting || _globalAutomaticCast)
                    castingModifier.TryCast((Being)TargetingSystem.CastTarget, true);
            }
        }

        /// <summary>
        ///     Manual Cast
        /// </summary>
        public bool CastModifier(string modifierId)
        {
            return CastModifier((Being)TargetingSystem.CastTarget, modifierId);
        }

        /// <summary>
        ///     Manual Cast
        /// </summary>
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

            return true;
        }

        public void AddCastModifier(Modifier modifier)
        {
            _castModifiers.Add(modifier);
        }

        public void RemoveCastModifier(Modifier modifier)
        {
            _castModifiers.Remove(modifier);
        }

        public void SetAutomaticCastAll(bool automaticCast = true)
        {
            _globalAutomaticCast = automaticCast;
        }
    }
}