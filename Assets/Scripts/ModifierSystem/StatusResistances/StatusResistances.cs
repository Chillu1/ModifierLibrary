using System.Collections.Generic;
using BaseProject;
using BaseProject.Utils;

namespace ModifierSystem
{
    public sealed class StatusResistances
    {
        private readonly ResourceController<string, StatusResistance> _controller;

        public StatusResistances()
        {
            List<StatusResistance> statusResistances = new List<StatusResistance>();

            SetupStatusResistance(statusResistances, StatusTypeHelper.StatusTypes);
            SetupStatusResistance(statusResistances, PositiveNegativeHelper.PositiveNegatives);
            SetupStatusResistance(statusResistances, DamageTypeHelper.DamageTypes);
            SetupStatusResistance(statusResistances, ElementalTypeHelper.ElementalTypes);

            _controller = new ResourceController<string, StatusResistance>(statusResistances);
        }

        private void SetupStatusResistance<T>(List<StatusResistance> list, T[] enumArray)
        {
            foreach (T statusResistance in enumArray)
            {
                if((int)(object)statusResistance == 0)
                    continue;

                if (Utilities.IsPowerOfTwo((ulong)(int)(object)statusResistance))
                    list.Add(new StatusResistance(statusResistance));
            }
        }

        public bool ChangeValue(StatusType statusType, double value) => ChangeValue(statusType.ToString(), value);
        public bool ChangeValue(PositiveNegative statusType, double value) => ChangeValue(statusType.ToString(), value);
        public bool ChangeValue(DamageType statusType, double value) => ChangeValue(statusType.ToString(), value);
        public bool ChangeValue(ElementalType statusType, double value) => ChangeValue(statusType.ToString(), value);

        private bool ChangeValue(string id, double value)
        {
            if (GetResistance(id, out var resistance))
                return resistance.Change(value);
            return false;
        }

        public void ChangeMultiplier(string id, double multiplier)
        {
            if (GetResistance(id, out var resistance))
                resistance.ChangeMultiplier(multiplier);
        }

        public bool HasValue(string id, double value)
        {
            if (GetResistance(id, out var resistance))
                return resistance.Has(value);
            return false;
        }

        public double GetStatusMultiplier(StatusTag[] statusTags)
        {
            double multiplier = 1d;
            if (statusTags != null)
            {
                foreach (var tag in statusTags)
                    multiplier *= GetStatusMultiplier(tag.GetTag());
            }

            return multiplier;
        }

        public double GetStatusMultiplier(StatusType id)
        {
            return GetStatusMultiplier(id.ToString());
        }

        public double GetStatusMultiplier(PositiveNegative id)
        {
            return GetStatusMultiplier(id.ToString());
        }

        public double GetStatusMultiplier(ElementalType id)
        {
            return GetStatusMultiplier(id.ToString());
        }

        public double GetStatusMultiplier(DamageType id)
        {
            return GetStatusMultiplier(id.ToString());
        }

        private double GetStatusMultiplier(object id)
        {
            return GetStatusMultiplier(id.ToString());
        }
        private double GetStatusMultiplier(string id)
        {
            double multiplier = 1d;
            if (GetResistance(id, out var resistance))
                multiplier = 1d - resistance.Percentage;

            return multiplier;
        }

        private bool GetResistance(string id, out StatusResistance resistance)
        {
            return _controller.GetResource(id, out resistance);
        }

        public override string ToString()
        {
            return string.Join(", ", _controller.Where(res => res.Percentage != 0d ));
        }
    }
}