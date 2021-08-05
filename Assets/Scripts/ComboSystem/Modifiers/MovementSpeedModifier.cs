namespace ComboSystem
{
    public class MovementSpeedModifier : Modifier<MovementSpeedModifierData>
    {
        protected override void Apply()
        {
            //Target.AddStat(StatTypes.MovementSpeed, data.movementSpeed);
        }

        protected override void Remove()
        {
            //Target.RemoveStat(StatTypes.MovementSpeed, data.movementSpeed);
        }

        //public IEnumerator UnapplicationCoroutine()
        //{
        //    yield return new WaitForSeconds(data.duration);
        //    Target.RemoveStrength(data.strengthToAdd);
        //}
    }
}