using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class StatusResistanceTest : ModifierBaseTest
    {
        private const double ResistanceValue = 1_000;
        private const double ResistanceMaxValue = double.MaxValue;

        [Test]
        public void NoStatusResDuration()
        {
            var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(10.1f); //Should remove modifier

            var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
            Assert.True(!enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void StatusResDuration()
        {
            var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.StatusResistances.ChangeValue(StatusType.DoT, ResistanceValue);
            character.Attack(enemy);
            enemy.Update((float)(10.1d * BaseProject.Curves.StatusResistance.Evaluate(ResistanceValue))); //Should remove modifier

            var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
            Assert.True(!enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void StatusResMaxDuration()
        {
            var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.StatusResistances.ChangeValue(StatusType.DoT, ResistanceMaxValue);
            character.Attack(enemy);
            enemy.Update((float)(10.1d * BaseProject.Curves.StatusResistance.Evaluate(ResistanceMaxValue))); //Should remove modifier

            var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
            Assert.True(!enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void StatusResCombinationDuration()
        {
            var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            enemy.StatusResistances.ChangeValue(StatusType.DoT, ResistanceValue);
            enemy.StatusResistances.ChangeValue(DamageType.Physical, ResistanceValue);
            enemy.StatusResistances.ChangeValue(ElementalType.Poison, ResistanceValue);
            character.Attack(enemy);
            enemy.Update((float)(10.1d * BaseProject.Curves.StatusResistance.Evaluate(ResistanceValue) *
                                 BaseProject.Curves.StatusResistance.Evaluate(ResistanceValue) *
                                 BaseProject.Curves.StatusResistance.Evaluate(ResistanceValue))); //Should remove modifier

            var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
            Assert.True(!enemy.ContainsModifier(doTModifier));
        }
    }
}