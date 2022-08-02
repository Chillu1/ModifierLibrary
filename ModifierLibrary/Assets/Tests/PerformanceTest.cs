using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnitLibrary;
using Force.DeepCloner;
using Newtonsoft.Json;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace ModifierLibrary.Tests.Performance
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
                    var modifier = new Modifier("IceBoltTest", null, AddModifierParameters.OwnerIsTarget);
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                    var target = new TargetComponent();
                    var effect = new DamageComponent(damageData);
                    var check = new CheckComponent(new IEffectComponent[] { effect });
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
        public void BenchNewModifierGenerationPropertiesOnly()
        {
            Measure.Method(() =>
                {
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                    var properties = new ModifierGenerationProperties("IceBoltTest", null);
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetRemovable();
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierGenerationOnly()
        {
            ModifierGenerationProperties properties = null;
            Measure.Method(() =>
                {
                    var modifier = ModifierGenerator.GenerateModifier(properties);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    var damageData = new[] { new DamageData(15, DamageType.Magical, new ElementData(ElementType.Cold, 20, 10)) };
                    properties = new ModifierGenerationProperties("IceBoltTest", null);
                    properties.AddEffect(new DamageComponent(damageData), damageData);
                    properties.SetEffectOnInit();
                    properties.SetRemovable();
                })
                .CleanUp(() => { properties = null; })
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchNewModifierGenerationComplex()
        {
            Measure.Method(() =>
                {
                    var damageData = new[] { new DamageData(1, DamageType.Physical, new ElementData(ElementType.Poison, 10, 20)) };
                    var properties = new ModifierGenerationProperties("DoTStackTest", null, LegalTarget.Units);
                    properties.AddEffect(new DamageComponent(damageData, DamageComponent.StackEffectType.Add), damageData);
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
        public void BenchNewUnit()
        {
            Measure.Method(() =>
                {
                    var character = new Unit(new UnitProperties
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
        public void BenchNewUnitClone()
        {
            var character = new Unit(new UnitProperties
            {
                Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
            });

            Measure.Method(() =>
                {
                    var clonedUnit = (Unit)character.Clone();
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchUnitNormalAttack()
        {
            Unit character = null, enemy = null;
            var logLevel = Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { character!.Attack(enemy); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    character = new Unit(new UnitProperties
                    {
                        Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                        Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
                    });
                    enemy = new Unit(new UnitProperties
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

            Log.ChangeCategoryLogLevel("modifiers", logLevel);
        }

        [Test, Performance]
        public void BenchUnitElementAttack()
        {
            Unit character = null, enemy = null;
            var logLevel = Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { character!.Attack(enemy); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    character = new Unit(new UnitProperties
                    {
                        Id = "player", Health = 50,
                        Damage = new DamageData(1, DamageType.Physical, new ElementData(ElementType.Fire, 20, 10)),
                        MovementSpeed = 3,
                        Mana = 100, ManaRegen = 50, UnitType = UnitType.Ally
                    });
                    enemy = new Unit(new UnitProperties
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

            Log.ChangeCategoryLogLevel("modifiers", logLevel);
        }

        [Test, Performance]
        public void BenchUnitNormalAttackWithAppliers()
        {
            Unit character = null, enemy = null;
            var logLevel = Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { character!.Attack(enemy); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    character = new Unit(new UnitProperties
                    {
                        Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                        Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
                    });
                    character.AddModifier(_modifierPrototypesTest.GetApplier("DoTStackTest"));
                    character.AddModifier(_modifierPrototypesTest.GetApplier("SilenceXStacksTest"));
                    enemy = new Unit(new UnitProperties
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

            Log.ChangeCategoryLogLevel("modifiers", logLevel);
        }

        [Test, Performance]
        public void BenchApplyModifier()
        {
            Modifier modifier = null;
            Unit character = null;
            var logLevel = Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() => { modifier!.TryApply(character); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    modifier = _modifierPrototypesTest.GetApplier("DoTStackTest");
                    character = new Unit(new UnitProperties
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

            Log.ChangeCategoryLogLevel("modifiers", logLevel);
        }

        [Test, Performance]
        public void BenchModifierPrototypesSetup()
        {
            var logLevel = Log.ChangeCategoryLogLevel("modifiers", LogLevel.Error);

            Measure.Method(() =>
                {
                    var modifierPrototypes = new ModifierPrototypes<Modifier>(true);
                })
                .WarmupCount(10)
                .MeasurementCount(5)
                .IterationsPerMeasurement(Iterations/4)
                .GC()
                .Run()
                ;

            Log.ChangeCategoryLogLevel("modifiers", logLevel);
        }

        [Test, Performance]
        public void BenchEffectComponentConstructor()
        {
            var damageType = DamageType.Physical;
            double value = 10d;

            Measure.Method(() =>
                {
                    var _ = new DamageResistanceComponent(damageType, value, null, false);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(200)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchEffectComponentActivator()
        {
            var damageType = DamageType.Physical;
            double value = 10d;

            Measure.Method(() =>
                {
                    var _ = Activator.CreateInstance(typeof(DamageResistanceComponent),
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null,
                        new object[] { damageType, value, null, false }, CultureInfo.CurrentCulture);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(200)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchUnitSaveJson()
        {
            JsonTextWriter writer = null;
            var unit = new Unit(new UnitProperties
            {
                Id = "player", Health = 50, Damage = new DamageData(1, DamageType.Physical, null), MovementSpeed = 3,
                Mana = 100, ManaRegen = 1, UnitType = UnitType.Ally
            });
            Measure.Method(() =>
                {
                    unit.Save(writer);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(Iterations)
                .GC()
                .SetUp(() =>
                {
                    writer = new JsonTextWriter(new StringWriter(new StringBuilder()));
                })
                .CleanUp(() =>
                {
                    writer = null;
                })
                .Run()
                ;
        }
    }
}