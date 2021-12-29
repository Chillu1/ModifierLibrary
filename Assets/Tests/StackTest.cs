using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class StackTest : ModifierBaseTest
    {
        [Test]
        public void DoTStackingDamage()
        {
            var doTModifierApplier = modifierPrototypes.GetItem("DoTStackTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            Log.Info("1: "+enemy.Stats.Health.CurrentHealth);
            character.Attack(enemy);//1 phys dmg, 5 poison dmg
            Log.Info("2: "+enemy.Stats.Health.CurrentHealth);
            enemy.Update(2.1f);//5 poison damage
            Log.Info("3: "+enemy.Stats.Health.CurrentHealth);
            character.Attack(enemy);//1 phys, stack (+5 poison dmg)
            Log.Info("4: "+enemy.Stats.Health.CurrentHealth);
            enemy.Update(2.1f);//10 poison dmg
            Log.Info("5: "+enemy.Stats.Health.CurrentHealth);

            var doTModifier = modifierPrototypes.GetItem("DoTStackTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
            Assert.AreEqual(initialHealthEnemy-1-5-5-1-10, enemy.Stats.Health.CurrentHealth, Delta);
        }
    }
}