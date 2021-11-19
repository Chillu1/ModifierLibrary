using BaseProject;

namespace ComboSystemComposition
{
    public class ModifierPrototypes : ModifierPrototypesBase<Modifier>
    {
        public ModifierPrototypes()
        {
            SetupModifierPrototypes();
        }

        protected sealed override void SetupModifierPrototypes()
        {
            //Super simple example modifier:
            //Single 100 damage ability on target, (lingers 0.5s, but no actual duration)
            //On Init, Apply It (deal damage), remove 0.5 seconds after from manager

            //IceBoltDebuff
            var iceBoltModifier = new Modifier("IceBolt");
            var iceBoltTarget = new TargetComponent(UnitType.Self);
            var iceBoltEffect = new DamageComponent(new []{new DamageData(100, DamageType.Cold)}, iceBoltTarget);
            var iceBoltApply = new ApplyComponent(iceBoltEffect, iceBoltTarget);
            iceBoltModifier.AddComponent(new TimeComponent(new RemoveComponent(iceBoltModifier)));
            iceBoltModifier.AddComponent(new InitComponent(iceBoltApply));

            //Forever buff (applier), not refreshable or stackable (for now)
            //Apply on attack

            //IceBoltApplier
            var iceBoltApplier = new Modifier("IceBoltApplier", true);
            var iceBoltApplierTarget = new TargetComponent(UnitType.DefaultOffensive);
            var iceBoltApplierEffect = new ApplierComponent(iceBoltModifier, iceBoltApplierTarget);
            var iceBoltApplierApply = new ApplyComponent(iceBoltApplierEffect, iceBoltApplierTarget);
            iceBoltApplier.AddComponent(iceBoltApplierApply);//TODO Apply component on attack


            //On apply/init, add attackSpeed & speed buffs, after 5 seconds, remove buff.
            //var aspectOfTheCatModifier = new Modifier("AspectOfTheCat");
            //var aspectOfTheCatBuff = new StatComponent( /*5 speed & 5 attackSpeed*/);
            //aspectOfTheCatModifier.AddComponent(new TimeComponent(5, new RemoveComponent(aspectOfTheCatModifier)));
            //aspectOfTheCatModifier.AddComponent(new InitComponent(new ApplyComponent(aspectOfTheCatBuff)));

            //Modifier doTModifier = new Modifier("RefreshedDoTIntervalDurationModifier");

            //Deal damage, without any condition
            /*var damageComponent = doTModifier.AddComponent(new EffectComponent(delegate
            {
                /*doTModifier.Target.DealDamage();#1#
            }));
            //Interval, every 1 sec call effect (damage)
            doTModifier.AddComponent(new TimeComponent(1, true, damageComponent));

            //Remove component
            var removeDot = doTModifier.AddComponent(new RemoveComponent(delegate
            {
                /*Remove logic? Should be generic?#1#
            }));*/
            //after 5 sec, remove modifier
            //doTModifier.AddComponent(new TimeComponent(5, false, removeDot));

            //Stack part
            //doTModifier.AddComponent(new StackComponent());

            //Graphics-, Audio-, Component, etc, whatever

            //ModifierSet modifierSet = new ModifierSet();



            //--OLD--//

            // DamageOverTimeData spiderPoisonData = new DamageOverTimeData(new Damages(5, DamageType.Poison), 1f, 5f);
            // DamageOverTimeModifier spiderPoisonModifier = new DamageOverTimeModifier("SpiderPoison", spiderPoisonData, ModifierProperties.Stackable);
            // SetupModifierApplier(spiderPoisonModifier);
            //
            // //Aspect of the cat base mods
            // var catMovementSpeedBuffData = new StatChangeModifierData(StatType.MovementSpeed, 3f);
            // var catMovementSpeedBuff = new StatChangeModifier("MovementSpeedOfCat", catMovementSpeedBuffData);
            // SetupModifier(catMovementSpeedBuff);
            // var catAttackSpeedBuffData = new StatChangeModifierData(StatType.AttackSpeed, 3f);
            // var catAttackSpeedBuff = new StatChangeModifier("AttackSpeedOfCat", catAttackSpeedBuffData);
            // SetupModifier(catAttackSpeedBuff);
        }
    }
}