using System.Collections.Generic;
using UnitLibrary;

namespace ModifierLibrary
{
	/// <summary>
	///     Responsible for casting modifiers on targets
	/// </summary>
	public class CastingController
	{
		private ModifierController ModifierController { get; }
		private IStatusEffects StatusEffects { get; }
		private ITargetingSystem TargetingSystem { get; }

		private readonly List<Modifier> _castModifiers;
		private readonly List<Modifier> _allyAuraCastModifiers;
		private readonly List<Modifier> _enemyAuraCastModifiers;

		private float _castTimer = AutomaticCastCooldown;
		private float _auraCastTimer = AutomaticAuraCastCooldown;
		public const float AutomaticCastCooldown = 0.1f;
		public const float AutomaticAuraCastCooldown = 1.0f;

		private bool _globalAutomaticCast;

		public CastingController(ModifierController modifierController, IStatusEffects statusEffects, ITargetingSystem targetingSystem)
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
			_auraCastTimer -= deltaTime;

			//Log.Info(_castModifiers.Count + " cast modifiers. " + _allyAuraCastModifiers.Count + " ally aura cast modifiers. " +
			//         _enemyAuraCastModifiers.Count + " enemy aura cast modifiers.");
			//Log.Info(TargetingSystem.AllyAuraTargets.Count + " ally aura targets. " + TargetingSystem.EnemyAuraTargets.Count +
			//         " enemy aura targets.");

			if (_castTimer <= 0)
			{
				_castTimer = AutomaticCastCooldown;

				foreach (var castModifier in _castModifiers)
				{
					if (castModifier.IsAutomaticActing || _globalAutomaticCast)
						castModifier.TryCast((Unit)TargetingSystem.CastTarget, true);
				}
			}

			if (_auraCastTimer > 0)
				return;

			_auraCastTimer = AutomaticAuraCastCooldown;

			AuraCast(_allyAuraCastModifiers, TargetingSystem.AllyAuraTargets);
			AuraCast(_enemyAuraCastModifiers, TargetingSystem.EnemyAuraTargets);

			void AuraCast(List<Modifier> modifiers, List<UnitLibrary.Unit> targets)
			{
				foreach (var castModifier in modifiers)
				{
					if (!castModifier.IsAutomaticActing && !_globalAutomaticCast)
						continue;

					foreach (var target in targets)
						castModifier.TryCast((Unit)target, true);
				}
			}
		}

		/// <summary>
		///     Manual Cast
		/// </summary>
		public bool CastModifier(string modifierId)
		{
			return CastModifier((Unit)TargetingSystem.CastTarget, modifierId);
		}

		/// <summary>
		///     Manual Cast
		/// </summary>
		public bool CastModifier(Modifier modifier)
		{
			return CastModifier((Unit)TargetingSystem.CastTarget, modifier);
		}

		/// <summary>
		///     Manual Cast
		/// </summary>
		public bool CastModifier(Unit target, string modifierId)
		{
			bool valid = ValidateCast(modifierId, out var modifier);
			if (!valid)
				return false;

			modifier.TryCast(target);
			return true;
		}

		/// <summary>
		///     Manual Cast
		/// </summary>
		public bool CastModifier(Unit target, Modifier modifier)
		{
			bool valid = ValidateCast(modifier.Id, out _);
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

		public void SetGlobalAutomaticCast(bool automaticCast = true)
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