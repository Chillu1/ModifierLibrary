using System;
using BaseProject;

namespace ModifierSystem
{
    public class StackComponent : Component, IStackComponent
    {
        private Action<DamageData[], double> PrototypeStackComponent { get; }
        private IMetaEffect MetaEffect{ get; set; }
        private ChangeType ChangeType { get; }
        private double Value { get; }

        private int Stacks { get; set; } = 1;
        private int MaxStacks { get; set; }

        public StackComponent(Action<DamageData[], double> prototypeStackComponent, int maxStacks)
        {
            PrototypeStackComponent = prototypeStackComponent;
            MaxStacks = maxStacks;
        }

        //public StackComponent(IMetaEffect metaEffect, int maxStacks, ChangeType changeType, double value)
        //{
        //    MetaEffect = metaEffect;
        //    MaxStacks = maxStacks;
        //    ChangeType = changeType;
        //    Value = value;
        //}

        public StackComponent(StackComponent prototypeStackComponent)
        {
            ChangeType = prototypeStackComponent.ChangeType;
            Value = prototypeStackComponent.Value;
            MaxStacks = prototypeStackComponent.MaxStacks;
        }

        public void Stack()
        {
            if (Stacks + 1 >= MaxStacks)
                return;
            //Log.Verbose($"Stacks: {Stacks}/{MaxStacks}", "modifiers");

            Stacks++;
            if (ChangeType == ChangeType.EveryXStack)
            {

            }
            MetaEffect.MetaEffect(ChangeType, Value);
        }

        /// <summary>
        ///     Can be caused by duration, timing out, etc
        /// </summary>
        public void RemoveStack()
        {
            if (Stacks - 1 < 1)
                return;

            Stacks--;
        }

        public void ResetStacks()
        {
            Stacks = 0;
        }
    }
    public interface IMetaEffect
    {
        void MetaEffect(ChangeType changeType, double value);
    }

    class MetaEffect //: IMetaEffect
    {
        private IEffectComponent _effectComponent;

        public MetaEffect(IEffectComponent effectComponent)
        {
            _effectComponent = effectComponent;
        }

        public void Change(ChangeType changeType)
        {
            switch (changeType)
            {
                case ChangeType.None:
                    break;
                case ChangeType.AdditiveIncrease:
                    break;
                case ChangeType.Multiply:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changeType), changeType, null);
            }
        }
    }

    public enum ChangeType
    {
        None = 0,
        AdditiveIncrease = 1,
        Multiply = 2,
        EveryXStack = 3,
    }
}