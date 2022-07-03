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

        private readonly List<Modifier> _castModifiers;
        private readonly List<Modifier> _allyAuraCastModifiers;
        private readonly List<Modifier> _enemyAuraCastModifiers;

        private float _castTimer = AutomaticCastCooldown;
        public const float AutomaticCastCooldown = 0.1f;

        private bool _globalAutomaticCast;

        public CastingController(ModifierController modifierController, StatusEffects statusEffects, TargetingSystem targetingSystem)
        {
            ModifierController = modifierController;
            StatusEffects = statusEffects;
            TargetingSystem = targetingSystem;

            _castModifiers = new List<Modifier>();
            _allyAuraCastModifiers = new List<Modifier>();
            _enemyAuraCastModifiers = new List<Modifier>();
        }

        public void Update(float deltaTime)
        {
            _castTimer -= deltaTime;
            if (_castTimer > 0)
                return;

            _castTimer = AutomaticCastCooldown;
            //Log.Info(_castModifiers.Count + " cast modifiers. " + _allyAuraCastModifiers.Count + " ally aura cast modifiers. " +
            //         _enemyAuraCastModifiers.Count + " enemy aura cast modifiers.");
            //Log.Info(TargetingSystem.AllyAuraTargets.Count + " ally aura targets. " + TargetingSystem.EnemyAuraTargets.Count +
            //         " enemy aura targets.");

            foreach (var castModifier in _castModifiers)
            {
                if (castModifier.IsAutomaticCasting || _globalAutomaticCast)
                    castModifier.TryCast((Being)TargetingSystem.CastTarget, true);
            }

            foreach (var castModifier in _allyAuraCastModifiers)
            {
                if (castModifier.IsAutomaticCasting || _globalAutomaticCast)
                {
                    foreach (var allyTarget in TargetingSystem.AllyAuraTargets)
                        castModifier.TryCast((Being)allyTarget, true);
                }
            }

            foreach (var castModifier in _enemyAuraCastModifiers)
            {
                if (castModifier.IsAutomaticCasting || _globalAutomaticCast)
                {
                    foreach (var enemyTarget in TargetingSystem.EnemyAuraTargets)
                        castModifier.TryCast((Being)enemyTarget, true);
                }
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
            bool valid = ValidateCast(modifierId, out var modifier);
            if (!valid)
                return false;

            modifier.TryCast(target);

            return true;
        }

        public void AddCastModifier(Modifier modifier)
        {
            if (modifier.ApplierType.HasFlag(ApplierType.Cast))
                _castModifiers.Add(modifier);
            if (!modifier.ApplierType.HasFlag(ApplierType.Aura))
                return;

            if (modifier.TargetComponent.LegalTarget.HasFlag(LegalTarget.Same))
                _allyAuraCastModifiers.Add(modifier);
            if (modifier.TargetComponent.LegalTarget.HasFlag(LegalTarget.Opposite))
                _enemyAuraCastModifiers.Add(modifier);
        }

        public void RemoveCastModifier(Modifier modifier)
        {
            _castModifiers.Remove(modifier);
            _allyAuraCastModifiers.Remove(modifier);
            _enemyAuraCastModifiers.Remove(modifier);
        }

        public void SetAutomaticCastAll(bool automaticCast = true)
        {
            _globalAutomaticCast = automaticCast;
        }

        private bool ValidateCast(string modifierId, out Modifier modifier)
        {
            if (!ModifierController.ContainsModifier(modifierId, out modifier))
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

            return true;
        }
    }
}