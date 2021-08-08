namespace ComboSystem
{
    public sealed class ModifierPrototypes : ModifierPrototypesBase<Modifier>
    {
        public ModifierPrototypes()
        {
            SetupModifierPrototypes();
        }

        protected override void SetupModifierPrototypes()
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

            //Aspect of the cat base mods
            var catMovementSpeedBuffData = new StatChangeModifierData(StatType.MovementSpeed, 3f);
            var catMovementSpeedBuff = new StatChangeModifier("MovementSpeedOfCat", catMovementSpeedBuffData);
            SetupModifier(catMovementSpeedBuff);
            var catAttackSpeedBuffData = new StatChangeModifierData(StatType.AttackSpeed, 3f);
            var catAttackSpeedBuff = new StatChangeModifier("AttackSpeedOfCat", catAttackSpeedBuffData);
            SetupModifier(catAttackSpeedBuff);
        }
    }
}