using BaseProject;

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
            DamageOverTimeData spiderPoisonData = new DamageOverTimeData(new Damages(5, DamageType.Poison), 1f, 5f);
            DamageOverTimeModifier spiderPoisonModifier = new DamageOverTimeModifier("SpiderPoison", spiderPoisonData, ModifierProperties.Stackable);
            SetupModifierApplier(spiderPoisonModifier);

            var fireAttackDoTData = new DamageOverTimeData(new Damages(2, DamageType.Fire), 1f, 5f);
            var fireAttackDoT = new DamageOverTimeModifier("FireDoTAttack", fireAttackDoTData, ModifierProperties.Refreshable);
            SetupModifierApplier(fireAttackDoT);

            var speedBuffDurationPlayerData = new StatChangeDurationModifierData(StatType.MovementSpeed, 2f, 3f);
            var speedBuffDurationPlayer = new StatChangeDurationModifier("PlayerMovementSpeedDurationBuff", speedBuffDurationPlayerData, ModifierProperties.Refreshable);
            SetupModifier(speedBuffDurationPlayer);

            var speedBuffDurationOnKillPlayerData = new StatChangeDurationModifierData(StatType.MovementSpeed, 2f, 5f);
            var speedBuffDurationOnKillPlayer = new StatChangeDurationModifier("PlayerMovementSpeedDurationOnKillBuff", speedBuffDurationOnKillPlayerData, ModifierProperties.Refreshable);
            SetupModifier(speedBuffDurationOnKillPlayer);

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

            var fireAttackData = new Damages(new DamageData() { Damage = 3f, DamageType = DamageType.Fire });
            var fireAttack = new DamageAttackModifier("FireDamage", fireAttackData);
            SetupModifierApplier(fireAttack);
            var coldAttackData = new Damages(new DamageData() { Damage = 3f, DamageType = DamageType.Cold });
            var coldAttack = new DamageAttackModifier("ColdDamage", coldAttackData);
            SetupModifierApplier(coldAttack);

            //var resurrectionData =
        }

        private void SetupModifierApplier(Modifier modifier)
        {
            var modifierApplierData = new ModifierApplierData(modifier);
            var modifierApplier = new ModifierApplier<ModifierApplierData>(modifier.Id+"Buff", modifierApplierData);
            SetupModifier(modifierApplier);
        }
    }
}