using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using BaseProject;

namespace ModifierSystem
{
    public static class EffectComponentFactory
    {
        private static readonly Dictionary<Type, Func<IEffectProperties, IBaseEffectProperties, EffectComponent>> cachedFuncs =
            new Dictionary<Type, Func<IEffectProperties, IBaseEffectProperties, EffectComponent>>();

        public static EffectComponent CreateEffectComponent<TEffectProperties>(TEffectProperties properties, IBaseEffectProperties baseProperties)
            where TEffectProperties : IEffectProperties
        {
            var propertyType = typeof(TEffectProperties);

            if (cachedFuncs.ContainsKey(propertyType))
            {
                return cachedFuncs[propertyType](properties, baseProperties);
            }
            
            var effectType = properties.GetType().DeclaringType;
            if (effectType == null)
            {
                Log.Error("Properties aren't nested, or this isn't an effectComponent", "modifiers");
                return null;
            }
            
            var ctorInfo1 = effectType.GetConstructor(new[]
            {
                propertyType, typeof(IBaseEffectProperties)
            });

            ParameterExpression paramA = Expression.Parameter(propertyType);
            ParameterExpression paramB = Expression.Parameter(typeof(IBaseEffectProperties));

            var newEffectGenerator =
                Expression.Lambda<Func<TEffectProperties, IBaseEffectProperties, EffectComponent>>(
                    Expression.New(ctorInfo1, new[]
                    {
                        paramA, paramB
                    }), paramA, paramB).Compile();
            cachedFuncs.Add(propertyType,
                (effectProperties, baseEffectProperties) => newEffectGenerator((TEffectProperties)effectProperties, baseEffectProperties));

            return newEffectGenerator(properties, baseProperties);
        }
        
        public static TEffect CreateEffectComponent<TEffectProperties, TEffect>(TEffectProperties properties, IBaseEffectProperties baseProperties)
            where TEffectProperties : IEffectProperties where TEffect : EffectComponent
        {
            var effectType = properties.GetType().DeclaringType;
            
            var ctorInfo1 = effectType.GetConstructor(new Type[]
            {
                typeof(TEffectProperties), typeof(IBaseEffectProperties)
            });

            ParameterExpression paramA = Expression.Parameter(typeof(TEffectProperties));
            ParameterExpression paramB = Expression.Parameter(typeof(IBaseEffectProperties));

            var testEffectGen =
                Expression.Lambda<Func<TEffectProperties, IBaseEffectProperties, TEffect>>(
                    Expression.New(ctorInfo1, new[]
                    {
                        paramA, paramB
                    }), paramA, paramB).Compile();

            return testEffectGen(properties, baseProperties);
        }
        
        public static TEffect CreateEffectComponent<TEffect>(IBaseEffectProperties baseProperties) where TEffect : EffectComponent
        {
            var ctorInfo1 = typeof(TEffect).GetConstructor(new[]
            {
                typeof(IBaseEffectProperties)
            });

            ParameterExpression paramA = Expression.Parameter(typeof(IBaseEffectProperties));

            var testEffectGen =
                Expression.Lambda<Func<IBaseEffectProperties, TEffect>>(
                    Expression.New(ctorInfo1, new[]
                    {
                        paramA
                    }), paramA).Compile();

            return testEffectGen(baseProperties);
        }

        public static EffectComponent CreateEffectComponent(Type effectComponentType, params object[] parameters)
        {
            EffectComponent effectComponent = (EffectComponent)Activator.CreateInstance(effectComponentType,
                BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding, null,
                parameters, CultureInfo.CurrentCulture);

            return effectComponent;
        }
    }
}