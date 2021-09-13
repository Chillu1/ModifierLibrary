using BaseProject;
using NUnit.Framework;

namespace ComboSystem.Tests
{
    public abstract class ModifierBaseTest
    {
        protected Character character;
        protected Character enemy;

        protected ModifierPrototypesTest modifierPrototypes;
        protected ComboModifierPrototypesTest comboModifierPrototypes;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            modifierPrototypes = new ModifierPrototypesTest();
            modifierPrototypes.AddTestModifiers();
            comboModifierPrototypes = new ComboModifierPrototypesTest();
        }

        [SetUp]
        public void Init()
        {
            character = new Character(new ComboBeingProperties() { Id = "player", Health = 50, Damage = 5, MovementSpeed = 3 });
            enemy = new Character(new ComboBeingProperties() { Id = "enemy", Health = 30, Damage = 3, MovementSpeed = 2 });
        }

        [TearDown]
        public void CleanUp()
        {
            character = null;
            enemy = null;
        }

        protected sealed class ModifierPrototypesTest : ModifierPrototypes
        {
            public void AddTestModifiers()
            {
                var physicalAttackDoTData = new DamageOverTimeData(new Damages(2, DamageType.Physical), 1f, 5f);
                var physicalAttackDoT = new DamageOverTimeModifier("PhysicalDoTAttack", physicalAttackDoTData, ModifierProperties.Refreshable);
                SetupModifierApplier(physicalAttackDoT);

                //var speedBuffDurationPlayerData = new StatChangeDurationModifierData(StatType.MovementSpeed, 2f, 3f);
                //var speedBuffDurationPlayer = new StatChangeDurationModifier("PlayerMovementSpeedDurationBuff", speedBuffDurationPlayerData, ModifierProperties.Refreshable);
                //SetupModifier(speedBuffDurationPlayer);
            }
        }

        protected sealed class ComboModifierPrototypesTest : ComboModifierPrototypes
        {
            public void AddTestModifiers()
            {
            }
        }
    }
}