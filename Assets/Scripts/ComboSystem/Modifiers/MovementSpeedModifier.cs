namespace ComboSystem
{
    public class MovementSpeedModifier : Modifier<MovementSpeedModifierData>
    {
        protected override void Apply()
        {
            //target.AddStat(StatTypes.MovementSpeed, data.movementSpeed);
        }

        protected override void Remove()
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