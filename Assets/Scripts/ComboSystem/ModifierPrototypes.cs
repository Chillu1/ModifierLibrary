using System.Collections.Generic;
using JetBrains.Annotations;

namespace ComboSystem
{
    public class ModifierPrototypes
    {
        private readonly Dictionary<string, Modifier> _modifierPrototypes;

        public ModifierPrototypes()
        {
            _modifierPrototypes = new Dictionary<string, Modifier>();
            SetupModifierPrototypes();
        }

        private void SetupModifierPrototypes()
        {
            DamageOverTimeData slimePoisonData =
                new DamageOverTimeData(new[] { new DamageData() { Damage = 5, DamageType = DamageType.Poison } }, 1f, 5f);
            DamageOverTimeModifier slimePoisonModifier = new DamageOverTimeModifier(slimePoisonData);
            SetupModifier("SlimePoison", slimePoisonModifier);

            var speedBuffPlayerData = new MovementSpeedDurationModifierData(2f, 10f);
            var speedBuffPlayer = new MovementSpeedDurationModifier(speedBuffPlayerData);
            SetupModifier("PlayerSpeedBuff", speedBuffPlayer);

            var poisonModifierBuffData = new ModifierApplierData(slimePoisonModifier);
            var poisonModifierBuff = new ModifierApplier<ModifierApplierData>(poisonModifierBuffData);
            SetupModifier("SlimePoisonBuff", poisonModifierBuff);

            void SetupModifier(string modifierName, Modifier modifier)
            {
                _modifierPrototypes.Add(modifierName, modifier);
            }
        }

        [CanBeNull]
        public Modifier GetModifier(string modifierName, ModifierController modifierController)
        {
            if (_modifierPrototypes.TryGetValue(modifierName, out Modifier modifier))
            {
                modifier = (Modifier)modifier.Clone();
                RegisterModifier(modifier, modifierController);
                return modifier;
            }
            else
            {
                Log.Error("Could not find modifier of name " + modifierName);
                return null;
            }
        }

        [CanBeNull]
        public Modifier<TDataType> GetModifier<TDataType>(string modifierName, ModifierController modifierController)
        {
            return (Modifier<TDataType>)GetModifier(modifierName, modifierController);
        }

        private void RegisterModifier(Modifier modifier, ModifierController modifierController)
        {
            modifier.Removed += modifierEventItem => modifierController.RemoveModifier(modifierEventItem);
        }
    }
}