using BaseProject;
using Force.DeepCloner;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace ModifierSystem.Tests.Performance
{
    public class PerformanceTest
    {
        /// <summary>
        ///     Const Iterations to make sure every bench uses this. It's needed to compare apples to apples.
        /// </summary>
        private const int Iterations = 100;

        private ModifierBaseTest.ModifierPrototypesTest _modifierPrototypesTest;

        [OneTimeSetUp]
        public void OneTimeInit()
        {
            _modifierPrototypesTest = new ModifierBaseTest.ModifierPrototypesTest();
        }

        [Test, Performance]
        public void BenchNewModifierManual()
        {
            Measure.Method(() =>
                {
                    var modifier = new Modifier("IceBoltTest", null);
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                    var target = new TargetComponent();
                    var effect = new DamageComponent(damageData);
                    var check = new CheckComponent(effect);
                    modifier.AddComponent(target);
                    modifier.AddComponent(new InitComponent(check));
                    modifier.AddComponent(new TimeComponent(new RemoveComponent(modifier)));
                    modifier.FinishSetup(damageData);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierCloneShallow()
        {
            Modifier modifier = null;

            Measure.Method(() =>
                {
                    var clonedModifier = modifier!.ShallowClone();
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() => { modifier = _modifierPrototypesTest.GetApplier("IceBoltTest"); })
                .CleanUp(() => { modifier = null; })
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierCloneDeep()
        {
            Modifier modifier = null;

            Measure.Method(() =>
                {
                    var clonedModifier = modifier!.DeepClone();
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() => { modifier = _modifierPrototypesTest.GetApplier("IceBoltTest"); })
                .CleanUp(() => { modifier = null; })
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierCloneProperties()
        {
            Modifier modifier = null;

            Measure.Method(() =>
                {
                    var clonedModifier = modifier!.PropertyClone();
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() => { modifier = _modifierPrototypesTest.GetApplier("IceBoltTest"); })
                .CleanUp(() => { modifier = null; })
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierGeneration()
        {
            Measure.Method(() =>
                {
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                    var properties = new ModifierGenerationProperties("IceBoltTest", null);
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetRemovable();

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierGenerationComplex()
        {
            Measure.Method(() =>
                {
                    var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                    var properties = new ModifierGenerationProperties("DoTStackTest", null, LegalTarget.Beings);
                    properties.AddEffect(new DamageComponent(damageData, DamageComponent.DamageComponentStackEffect.Add), damageData);
                    properties.SetEffectOnInit();
                    properties.SetEffectOnTime(2, true);
                    properties.SetEffectOnStack(new StackComponentProperties() { Value = 2, MaxStacks = 1000 });
                    properties.SetRemovable(10);

                    var modifier = ModifierGenerator.GenerateModifier(properties);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewBeing()
        {
            Measure.Method(() =>
                {
                    var character = new Being(new BeingProperties
                    {
                        Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                        Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
                    });
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewBeingClone()
        {
            var character = new Being(new BeingProperties
            {
                Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
            });

            Measure.Method(() =>
                {
                    var clonedBeing = (Being)character.Clone();
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchBeingNormalAttack()
        {
            Being character = null, enemy = null;
            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { character!.Attack(enemy); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    character = new Being(new BeingProperties
                    {
                        Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                        Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
                    });
                    enemy = new Being(new BeingProperties
                    {
                        Id = "enemy", Health = 30, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 2,
                        Mana = 20, ManaRegen = 1, UnitType = UnitType.Enemy
                    });
                })
                .CleanUp(() =>
                {
                    character = null;
                    enemy = null;
                })
                .Run()
                ;

            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Verbose);
        }

        [Test, Performance]
        public void BenchBeingElementAttack()
        {
            Being character = null, enemy = null;
            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { character!.Attack(enemy); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    character = new Being(new BeingProperties
                    {
                        Id = "player", Health = 50,
                        Damage = new DamageData(1, DamageType.Physical, new ElementData(ElementType.Fire, 20, 10)),
                        MovementSpeed = 3,
                        Mana = 100, ManaRegen = 50, UnitType = UnitType.Ally
                    });
                    enemy = new Being(new BeingProperties
                    {
                        Id = "enemy", Health = 30, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 2,
                        Mana = 20, ManaRegen = 1, UnitType = UnitType.Enemy
                    });
                })
                .CleanUp(() =>
                {
                    character = null;
                    enemy = null;
                })
                .Run()
                ;

            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Verbose);
        }

        [Test, Performance]
        public void BenchBeingNormalAttackWithAppliers()
        {
            Being character = null, enemy = null;
            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { character!.Attack(enemy); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    character = new Being(new BeingProperties
                    {
                        Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                        Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
                    });
                    character.AddModifier(_modifierPrototypesTest.GetApplier("DoTStackTest"));
                    character.AddModifier(_modifierPrototypesTest.GetApplier("SilenceXStacksTest"));
                    enemy = new Being(new BeingProperties
                    {
                        Id = "enemy", Health = 30, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 2,
                        Mana = 20, ManaRegen = 1, UnitType = UnitType.Enemy
                    });
                })
                .CleanUp(() =>
                {
                    character = null;
                    enemy = null;
                })
                .Run()
                ;

            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Verbose);
        }

        [Test, Performance]
        public void BenchApplyModifier()
        {
            Modifier modifier = null;
            Being character = null;
            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { modifier!.TryApply(character); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    modifier = _modifierPrototypesTest.GetApplier("DoTStackTest");
                    character = new Being(new BeingProperties
                    {
                        Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                        Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
                    });
                })
                .CleanUp(() =>
                {
                    character = null;
                    modifier = null;
                })
                .Run()
                ;

            Log.ChangeCategoryLogLevel("modifiers", LogLevel.Verbose);
        }
    }
}