using System;

//Design:
//Clean interaction between applied buffs on unit
//All buffs in a single collection, ticking
//Calculate base stats, then add percentages, every time our stats change, recalculate
namespace ComboSystemTest
{
    public enum StatTypes
    {
        None = 0,
        Attack = 1,
        Defense = 2,
        MovementSpeed = 3,
    }

    [Flags]
    public enum DamageType
    {
        None = 0,
        Physical = 1,
        Magical = 2,
        Poison = 4,
        Fire = 8,
        Acid = 16,
    }

    public class ModifierFactory<TDataType, TModifierType> where TModifierType : Modifier<TDataType>, new()
    {
        public TDataType data;

        public Modifier GetModifier(ICharacter target)
        {
            return new TModifierType {data = this.data, target = target};
        }
    }

    public interface ICharacter
    {
    }

    public abstract class Modifier
    {
        public abstract void Apply();

        public virtual void Remove()
        {
        }

        public virtual void Update(float deltaTime)
        {
        }
    }

    public abstract class Modifier<TDataType> : Modifier
    {
        public TDataType data;
        public ICharacter target;
    }

    public class DamageOverTimeData
    {
        public int damage = 5;
        public float everyXSecond = 1f;
        public float duration = 5f;
        //elemental
    }

    public class DamageOverTimeModifier : Modifier<DamageOverTimeData>
    {
        private float _timer;

        public override void Apply()
        {
            //target.DealDamage(data.elementalType/*(damageType too, physical, magical*/, data.damage);
        }

        public override void Update(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= data.everyXSecond)
            {
                Apply();
                _timer = 0;
            }

            base.Update(deltaTime);
        }
    }

    public class MovementSpeedModifierData
    {
        public int movementSpeed = 2;
        public float duration = 5f;
    }

    public class MovementSpeedModifier : Modifier<MovementSpeedModifierData>
    {
        public override void Apply()
        {
            //target.AddStat(StatTypes.MovementSpeed, data.movementSpeed);
        }

        public override void Remove()
        {
            //target.RemoveStat(StatTypes.MovementSpeed, data.movementSpeed);
        }

        //public IEnumerator UnapplicationCoroutine()
        //{
        //    yield return new WaitForSeconds(data.duration);
        //    target.RemoveStrength(data.strengthToAdd);
        //}
    }
}