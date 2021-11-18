using BaseProject;

namespace ComboSystem
{
    public class ModifierPrototypes : ModifierPrototypesBase<Modifier>
    {
        public ModifierPrototypes()
        {
            SetupModifierPrototypes();
        }

        protected sealed override void SetupModifierPrototypes()
        {
            DamageOverTimeData spiderPoisonData = new DamageOverTimeData(new []{new DamageData(1, DamageType.Poison)}, 1f, 10f);
            DamageOverTimeModifier spiderPoisonModifier = new DamageOverTimeModifier("SpiderPoison", spiderPoisonData, ModifierProperties.Stackable);
            SetupModifierApplier(spiderPoisonModifier);

            var fireAttackDoTData = new DamageOverTimeData(new []{new DamageData(2, DamageType.Fire)}, 1f, 5f);
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

            var fireAttackData = new []{new DamageData(3, DamageType.Fire)};
            var fireAttack = new DamageAttackModifier("FireDamage", fireAttackData);
            SetupModifierApplier(fireAttack);
            var coldAttackData = new []{new DamageData(3, DamageType.Cold)};
            var coldAttack = new DamageAttackModifier("ColdDamage", coldAttackData);
            SetupModifierApplier(coldAttack);

            var coldResistanceData = new ResistanceDurationModifierData(5f, DamageType.Cold, 10d, 3d);
            var coldResistance = new ResistanceDurationModifier("ColdResistance", coldResistanceData);
            SetupModifier(coldResistance);

            var energyShieldResistanceOnAttackedData = new ResistanceModifierData(DamageType.Energy, 100d);
            var energyShieldResistanceOnAttacked = new SingleUseResistanceModifier("EnergyShield", energyShieldResistanceOnAttackedData);
            SetupModifier(energyShieldResistanceOnAttacked);

            //var resurrectionData =
        }

        protected void SetupModifierApplier(Modifier modifier)
        {
            var modifierApplierData = new ModifierApplierData(modifier);
            var modifierApplier = new ModifierApplier<ModifierApplierData>(modifier.Id+"Applier", modifierApplierData);
            SetupModifier(modifierApplier);
        }
    }
}