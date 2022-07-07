using System;
using System.Globalization;
using System.Reflection;
using BaseProject;

namespace ModifierSystem
{
    public static class EffectComponentFactory
    {
        public static EffectComponent CreateEffectComponent(Type effectComponentType, params object[] parameters)
        {
            EffectComponent effectComponent = (EffectComponent)Activator.CreateInstance(effectComponentType,
                BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding, null,
                parameters, CultureInfo.CurrentCulture);
            //Activator.CreateInstance(effectComponentType, parameters);
            
            
            return effectComponent;
        }
    }

    public struct DamageResistanceParameters
    {
        public DamageType DamageType { get; }
        public double Value { get; }

        public DamageResistanceParameters(DamageType damageType, double value) : this()
        {
            DamageType = damageType;
            Value = value;
        }
    }
}