namespace ComboSystem
{
    public class ModifierApplierData
    {
        public Modifier Modifier { get; protected set; }

        public ModifierApplierData(Modifier modifier)
        {
            Modifier = modifier;
        }
    }
}