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
            DamageOverTimeModifier slimePoisonModifier = new DamageOverTimeModifier("SlimePoison", slimePoisonData, ModifierProperties.Stackable);
            SetupModifier(slimePoisonModifier);

            var speedBuffDurationPlayerData = new StatChangeDurationModifierData(StatType.MovementSpeed, 2f, 3f);
            var speedBuffDurationPlayer = new StatChangeDurationModifier("PlayerMovementSpeedDurationBuff", speedBuffDurationPlayerData, ModifierProperties.Refreshable);
            SetupModifier(speedBuffDurationPlayer);

            var poisonModifierBuffData = new ModifierApplierData(slimePoisonModifier);
            var poisonModifierBuff = new ModifierApplier<ModifierApplierData>("SlimePoisonBuff", poisonModifierBuffData);
            SetupModifier(poisonModifierBuff);

            var speedBuffData = new StatChangeModifierData(StatType.MovementSpeed, 3f);
            var speedBuff = new StatChangeModifier("MovementSpeedBuff", speedBuffData);
            SetupModifier(speedBuff);

            var refreshableSpeedBuffData = new StatChangeModifierData(StatType.MovementSpeed, 3f);
            var refreshableSpeedBuff = new StatChangeModifier("RefreshableMovementSpeedBuff", refreshableSpeedBuffData);
            SetupModifier(refreshableSpeedBuff);

            var stackableSpeedBuffData = new StatChangeStacksModifierData(StatType.MovementSpeed, 7f, 3);
            var stackableSpeedBuff = new StatChangeStacksModifier("StackableMovementSpeedBuff", stackableSpeedBuffData);
            SetupModifier(stackableSpeedBuff);

            void SetupModifier(Modifier modifier)
            {
                _modifierPrototypes.Add(modifier.Id, modifier);
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