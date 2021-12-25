using System;
using BaseProject;

namespace ModifierSystem
{
    public class StatusTag
    {
        private StatusType StatusType { get; }
        private PositiveNegative PositiveNegative { get; }
        private DamageType DamageType { get; }
        private ElementalType ElementalType { get; }

        public StatusTag(StatusType statusType)
        {
            StatusType = statusType;
        }
        public StatusTag(PositiveNegative positiveNegative)
        {
            PositiveNegative = positiveNegative;
        }
        public StatusTag(ElementalType elementalType)
        {
            ElementalType = elementalType;
        }
        public StatusTag(DamageType damageType)
        {
            DamageType = damageType;
        }

        public bool Contains(StatusType statusType) => StatusType == statusType;
        public bool Contains(PositiveNegative positiveNegative) => PositiveNegative == positiveNegative;
        public bool Contains(ElementalType elementalType) => ElementalType == elementalType;
        public bool Contains(DamageType damageType) => DamageType == damageType;

        public Enum GetTag()
        {
            if (StatusType != StatusType.None)
                return StatusType;
            if (PositiveNegative != PositiveNegative.None)
                return PositiveNegative;
            if (DamageType != DamageType.None)
                return DamageType;
            if (ElementalType != ElementalType.None)
                return ElementalType;

            return null;
        }

        protected bool Equals(StatusTag other)
        {
            return StatusType == other.StatusType && PositiveNegative == other.PositiveNegative && DamageType == other.DamageType && ElementalType == other.ElementalType;
        }

        public override string ToString()
        {
            if (StatusType != StatusType.None)
                return StatusType.ToString();
            if (PositiveNegative != PositiveNegative.None)
                return PositiveNegative.ToString();
            if (DamageType != DamageType.None)
                return DamageType.ToString();
            if (ElementalType != ElementalType.None)
                return ElementalType.ToString();

            return "Null, invalid tag";
        }
    }
}