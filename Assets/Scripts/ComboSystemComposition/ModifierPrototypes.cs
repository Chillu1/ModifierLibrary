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
            var iceBoltEffect = new DamageComponent(new []{new DamageData(15, DamageType.Cold)}, iceBoltTarget);
            var iceBoltApply = new ApplyComponent(iceBoltEffect, iceBoltTarget);
            iceBoltModifier.AddComponent(new InitComponent(iceBoltApply));
            iceBoltModifier.AddComponent(iceBoltTarget);
            iceBoltModifier.AddComponent(new TimeComponent(new RemoveComponent(iceBoltModifier)));
            SetupModifier(iceBoltModifier);

            //Forever buff (applier), not refreshable or stackable (for now)
            //Apply on attack

            //IceBoltApplier
            //var iceBoltApplier = new Modifier("IceBoltApplier", true);
            //var iceBoltApplierTarget = new TargetComponent(UnitType.DefaultOffensive);
            //var iceBoltApplierEffect = new ApplierComponent(iceBoltModifier, iceBoltApplierTarget);
            //var iceBoltApplierApply = new ApplyComponent(iceBoltApplierEffect, iceBoltApplierTarget);
            //iceBoltApplier.AddComponent(iceBoltApplierApply);//TODO Apply component on attack
            SetupModifierApplier(iceBoltModifier, UnitType.DefaultOffensive);

            //StackableSpiderPoison, removed after 10 seconds
            //-Each stack increases DoT damage by 2
            //-Each stack increases current duration by 2, to max 10 stacks
            var spiderPoisonModifier = new Modifier("SpiderPoison");
            var spiderPoisonTarget = new TargetComponent(UnitType.Self);
            var damageData = new[] { new DamageData(5, DamageType.Poison) };
            var spiderPoisonEffect = new DamageComponent(damageData, spiderPoisonTarget);
            var spiderPoisonStack = new StackComponent(() => damageData[0].BaseDamage += 2, 10);
            var spiderPoisonApply = new ApplyComponent(spiderPoisonEffect, spiderPoisonTarget);
            spiderPoisonModifier.AddComponent(new InitComponent(spiderPoisonApply));//Apply first stack/damage on init
            spiderPoisonModifier.AddComponent(spiderPoisonTarget);
            spiderPoisonModifier.AddComponent(new TimeComponent(spiderPoisonEffect, 2, true));//Every 2 seconds, deal 5 damage
            spiderPoisonModifier.AddComponent(new TimeComponent(new RemoveComponent(spiderPoisonModifier), 10));//Remove after 10 secs
            spiderPoisonModifier.AddComponent(spiderPoisonStack);
            SetupModifier(spiderPoisonModifier);

            SetupModifierApplier(spiderPoisonModifier, UnitType.DefaultOffensive);

            //On apply/init, add attackSpeed & speed buffs, after 5 seconds, remove buff.
            //var aspectOfTheCatModifier = new Modifier("AspectOfTheCat");
            //var aspectOfTheCatBuff = new StatComponent( /*5 speed & 5 attackSpeed*/);
            //aspectOfTheCatModifier.AddComponent(new TimeComponent(5, new RemoveComponent(aspectOfTheCatModifier)));
            //aspectOfTheCatModifier.AddComponent(new InitComponent(new ApplyComponent(aspectOfTheCatBuff)));


            //Graphics-, Audio-, Component, etc, whatever
        }

        //Generic non-removable applier, for now
        private void SetupModifierApplier(Modifier appliedModifier, UnitType unitType = UnitType.DefaultOffensive)
        {
            var modifierApplier = new Modifier(appliedModifier.Id+"Applier", true);
            var modifierApplierTarget = new TargetComponent(unitType, true);
            var modifierApplierEffect = new ApplierComponent(appliedModifier, modifierApplierTarget);
            var modifierApplierApply = new ApplyComponent(modifierApplierEffect, modifierApplierTarget);
            modifierApplier.AddComponent(modifierApplierApply);
            modifierApplier.AddComponent(modifierApplierTarget);
            SetupModifier(modifierApplier);
        }
    }
}