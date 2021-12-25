using BaseProject;

namespace ModifierSystem
{
    public sealed class StatusResistance : IResource<string>
    {
        //Only one of these four enums can be of value/changed, resistance is invalid if not
        public StatusType StatusType { get; }
        public PositiveNegative PositiveNegative { get; }
        public DamageType DamageType { get; }
        public ElementalType ElementalType { get; }

        public double Percentage { get; private set; }
        public double Multiplier { get; private set; } = 1d;//TODO Probably remove
        public bool IsImmune { get; private set; }//TODO Remove?

        public string Id => _resource.Id;
        public double Value => _resource.Value;

        private Resource<string> _resource;

        public StatusResistance(object statusType)
        {
            _resource = new Resource<string>(statusType.ToString());
            switch (statusType)
            {
                case StatusType t:
                    StatusType = t;
                    break;
                case PositiveNegative p:
                    PositiveNegative = p;
                    break;
                case ElementalType e:
                    ElementalType = e;
                    break;
                case DamageType d:
                    DamageType = d;
                    break;
                default:
                    Log.Error($"Wrong type of statusType: {statusType.GetType()}");
                    break;
            }

            Validate();
        }

        /*public StatusResistance(StatusType statusType)
        {
            _resource = new Resource<string>(statusType.ToString());
            StatusType = statusType;
            Validate();
        }
        public StatusResistance(PositiveNegative positiveNegative)
        {
            _resource = new Resource<string>(positiveNegative.ToString());
            PositiveNegative = positiveNegative;
            Validate();
        }
        public StatusResistance(ElementalType elementalType)
        {
            _resource = new Resource<string>(elementalType.ToString());
            ElementalType = elementalType;
            Validate();
        }
        public StatusResistance(DamageType damageType)
        {
            _resource = new Resource<string>(damageType.ToString());
            DamageType = damageType;
            Validate();
        }*/

        private bool Validate()
        {
            int amountOfStatuses = 0;
            if (StatusType != StatusType.None)
                amountOfStatuses++;
            if (PositiveNegative != PositiveNegative.None)
                amountOfStatuses++;
            if (DamageType != DamageType.None)
                amountOfStatuses++;
            if (ElementalType != ElementalType.None)
                amountOfStatuses++;

            if (amountOfStatuses != 1)
            {
                Log.Error($"Wrong amount of statuses, on ID: {Id}");
                return false;
            }

            return true;
        }

        public void SetImmune(bool isImmune)
        {
            IsImmune = isImmune;
            CalculatePercentage();
        }

        public bool Change(double value)
        {
            if (value == 0d)
            {
                Log.Error("Amount passed was 0, id: "+Id, "baseproject");
                return false;
            }

            if (!_resource.Change(value))
                return false;

            CalculatePercentage();
            return true;
        }

        public void ChangeMultiplier(double multiplier)
        {
            Multiplier += multiplier;
        }

        private void CalculatePercentage()
        {
            if (IsImmune)
            {
                Percentage = 1d;
                return;
            }

            Percentage = Curves.StatusResistance.Evaluate(Value * Multiplier);
        }

        public bool Check(double value)
        {
            //We should never "check" resistance, TODO TEMP FIX
            return true;//_resource.Check(value);
        }

        public bool Has(double value)
        {
            return _resource.Has(value);
        }

        public override string ToString()
        {
            return $"{Id}, percentage: {(Percentage*100d).ToString("F3")}%, value: {Value}, multiplier: {Multiplier}";
        }
    }
}