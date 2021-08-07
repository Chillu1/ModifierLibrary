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

            var speedBuffDurationPlayerData = new MovementSpeedDurationModifierData(2f, 3f);
            var speedBuffDurationPlayer = new MovementSpeedDurationModifier(speedBuffDurationPlayerData);
            SetupModifier("PlayerMovementSpeedDurationBuff", speedBuffDurationPlayer);

            var poisonModifierBuffData = new ModifierApplierData(slimePoisonModifier);
            var poisonModifierBuff = new ModifierApplier<ModifierApplierData>(poisonModifierBuffData);
            SetupModifier("SlimePoisonBuff", poisonModifierBuff);

            var speedBuffData = new MovementSpeedModifierData(3f);
            var speedBuff = new MovementSpeedModifier(speedBuffData);
            SetupModifier("MovementSpeedBuff", speedBuff);

            var refreshableSpeedBuffData = new MovementSpeedModifierData(3f, ModifierProperties.Refreshable);
            var refreshableSpeedBuff = new MovementSpeedModifier(refreshableSpeedBuffData);
            SetupModifier("RefreshableMovementSpeedBuff", refreshableSpeedBuff);

            var refreshableSpeedBuffData2 = new MovementSpeedModifierData(2f, ModifierProperties.Refreshable);
            var refreshableSpeedBuff2 = new MovementSpeedModifier(refreshableSpeedBuffData2);
            SetupModifier("RefreshableMovementSpeedBuff2", refreshableSpeedBuff2);

            void SetupModifier(string modifierName, Modifier modifier)
            {
                _modifierPrototypes.Add(modifierName, modifier);
            }
        }

        [CanBeNull]
        public Modifier GetModifier(string modifierName)
        {
            if (_modifierPrototypes.TryGetValue(modifierName, out Modifier modifier))
            {
                modifier = (Modifier)modifier.Clone();
                return modifier;
            }
            else
            {
                Log.Error("Could not find modifier of name " + modifierName);
                return null;
            }
        }

        [CanBeNull]
        public Modifier<TDataType> GetModifier<TDataType>(string modifierName)
        {
            return (Modifier<TDataType>)GetModifier(modifierName);
        }
    }
}