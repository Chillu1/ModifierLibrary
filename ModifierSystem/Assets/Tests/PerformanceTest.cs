using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
                .IterationsPerMeasurement(100)
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
                    var _ = new TestEffect(damageType, value);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(1000)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchEffectComponentLambdaExpression()
        {
            //var ctorInfo1 = typeof(DamageResistanceComponent).GetConstructor(new Type[] {
            //    typeof(DamageType), typeof(double), typeof(ConditionCheckData), typeof(bool)
            //});
            //
            //ParameterExpression paramA = Expression.Parameter(typeof(DamageType)/*, "damageType"*/);
            //ParameterExpression paramB = Expression.Parameter(typeof(double)/*, "value"*/);
            //ParameterExpression paramC = Expression.Parameter(typeof(ConditionCheckData)/*, "value"*/);
            //ParameterExpression paramD = Expression.Parameter(typeof(bool)/*, "value"*/);
            //
            //Func<DamageType, double, ConditionCheckData, bool, DamageResistanceComponent> testEffectGen =
            //    Expression.Lambda<Func<DamageType, double, ConditionCheckData, bool, DamageResistanceComponent>>(Expression.New(ctorInfo1, new[]
            //{
            //    paramA, paramB, paramC, paramD
            //}), paramA, paramB, paramC, paramD).Compile();

            var ctorInfo1 = typeof(TestEffect).GetConstructor(new Type[]
            {
                typeof(DamageType), typeof(double)//, typeof(ConditionCheckData)
            });

            ParameterExpression paramA = Expression.Parameter(typeof(DamageType) /*, "damageType"*/);
            ParameterExpression paramB = Expression.Parameter(typeof(double) /*, "value"*/);
            //ParameterExpression paramC = Expression.Parameter(typeof(ConditionCheckData) /*, "value"*/);

            Func<DamageType, double, TestEffect> testEffectGen =
                Expression.Lambda<Func<DamageType, double, TestEffect>>(Expression.New(ctorInfo1, new[]
                {
                    paramA, paramB
                }), paramA, paramB).Compile();

            var type = typeof(DamageResistanceComponent);
            var damageType = DamageType.Physical;
            double value = 10d;

            Measure.Method(() =>
                {
                    var _ = testEffectGen(damageType, value);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(1000)
                .GC()
                .Run()
                ;
        }

        [Test, Performance]
        public void BenchEffectComponentInstanceFactory()
        {
            var damageType = DamageType.Physical;
            double value = 10d;
            var _ = InstanceFactory.CreateInstance(typeof(TestEffect), damageType, value);

            Measure.Method(() =>
                {
                    var _ = InstanceFactory.CreateInstance(typeof(TestEffect), damageType, value);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(1000)
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
                    var _ = Activator.CreateInstance(typeof(TestEffect),
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null,
                        new object[] { damageType, value }, CultureInfo.CurrentCulture);
                })
                .WarmupCount(10)
                .MeasurementCount(20)
                .IterationsPerMeasurement(100)
                .GC()
                .Run()
                ;
            
        }
    }

    public static class InstanceFactory
    {
        private delegate object CreateDelegate(Type type, object arg1, object arg2, object arg3);

        private static ConcurrentDictionary<Tuple<Type, Type, Type, Type>, CreateDelegate> cachedFuncs =
            new ConcurrentDictionary<Tuple<Type, Type, Type, Type>, CreateDelegate>();

        public static object CreateInstance(Type type)
        {
            return InstanceFactoryGeneric<TypeToIgnore, TypeToIgnore, TypeToIgnore>.CreateInstance(type, null, null, null);
        }

        public static object CreateInstance<TArg1>(Type type, TArg1 arg1)
        {
            return InstanceFactoryGeneric<TArg1, TypeToIgnore, TypeToIgnore>.CreateInstance(type, arg1, null, null);
        }

        public static object CreateInstance<TArg1, TArg2>(Type type, TArg1 arg1, TArg2 arg2)
        {
            return InstanceFactoryGeneric<TArg1, TArg2, TypeToIgnore>.CreateInstance(type, arg1, arg2, null);
        }

        public static object CreateInstance<TArg1, TArg2, TArg3>(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return InstanceFactoryGeneric<TArg1, TArg2, TArg3>.CreateInstance(type, arg1, arg2, arg3);
        }

        public static object CreateInstance(Type type, params object[] args)
        {
            if (args == null)
                return CreateInstance(type);

            if (args.Length > 3 ||
                (args.Length > 0 && args[0] == null) ||
                (args.Length > 1 && args[1] == null) ||
                (args.Length > 2 && args[2] == null))
            {
                return Activator.CreateInstance(type, args);
            }

            var arg0 = args.Length > 0 ? args[0] : null;
            var arg1 = args.Length > 1 ? args[1] : null;
            var arg2 = args.Length > 2 ? args[2] : null;

            var key = Tuple.Create(
                type,
                arg0?.GetType() ?? typeof(TypeToIgnore),
                arg1?.GetType() ?? typeof(TypeToIgnore),
                arg2?.GetType() ?? typeof(TypeToIgnore));

            if (cachedFuncs.TryGetValue(key, out CreateDelegate func))
                return func(type, arg0, arg1, arg2);
            else
                return CacheFunc(key)(type, arg0, arg1, arg2);
        }

        private static CreateDelegate CacheFunc(Tuple<Type, Type, Type, Type> key)
        {
            var types = new Type[] { key.Item1, key.Item2, key.Item3, key.Item4 };
            var method = typeof(InstanceFactory).GetMethods()
                .Where(m => m.Name == "CreateInstance")
                .Where(m => m.GetParameters().Count() == 4).Single();
            var generic = method.MakeGenericMethod(new Type[] { key.Item2, key.Item3, key.Item4 });

            var paramExpr = new List<ParameterExpression>();
            paramExpr.Add(Expression.Parameter(typeof(Type)));
            for (int i = 0; i < 3; i++)
                paramExpr.Add(Expression.Parameter(typeof(object)));

            var callParamExpr = new List<Expression>();
            callParamExpr.Add(paramExpr[0]);
            for (int i = 1; i < 4; i++)
                callParamExpr.Add(Expression.Convert(paramExpr[i], types[i]));

            var callExpr = Expression.Call(generic, callParamExpr);
            var lambdaExpr = Expression.Lambda<CreateDelegate>(callExpr, paramExpr);
            var func = lambdaExpr.Compile();
            cachedFuncs.TryAdd(key, func);
            return func;
        }
    }

    public static class InstanceFactoryGeneric<TArg1, TArg2, TArg3>
    {
        private static ConcurrentDictionary<Type, Func<TArg1, TArg2, TArg3, object>> cachedFuncs =
            new ConcurrentDictionary<Type, Func<TArg1, TArg2, TArg3, object>>();

        public static object CreateInstance(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            if (cachedFuncs.TryGetValue(type, out Func<TArg1, TArg2, TArg3, object> func))
                return func(arg1, arg2, arg3);
            else
                return CacheFunc(type, arg1, arg2, arg3)(arg1, arg2, arg3);
        }

        private static Func<TArg1, TArg2, TArg3, object> CacheFunc(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            var constructorTypes = new List<Type>();
            if (typeof(TArg1) != typeof(TypeToIgnore))
                constructorTypes.Add(typeof(TArg1));
            if (typeof(TArg2) != typeof(TypeToIgnore))
                constructorTypes.Add(typeof(TArg2));
            if (typeof(TArg3) != typeof(TypeToIgnore))
                constructorTypes.Add(typeof(TArg3));

            var parameters = new List<ParameterExpression>()
            {
                Expression.Parameter(typeof(TArg1)),
                Expression.Parameter(typeof(TArg2)),
                Expression.Parameter(typeof(TArg3)),
            };

            var constructor = type.GetConstructor(constructorTypes.ToArray());
            var constructorParameters = parameters.Take(constructorTypes.Count).ToList();
            var newExpr = Expression.New(constructor, constructorParameters);
            var lambdaExpr = Expression.Lambda<Func<TArg1, TArg2, TArg3, object>>(newExpr, parameters);
            var func = lambdaExpr.Compile();
            cachedFuncs.TryAdd(type, func);
            return func;
        }
    }

    public class TypeToIgnore
    {
    }
}