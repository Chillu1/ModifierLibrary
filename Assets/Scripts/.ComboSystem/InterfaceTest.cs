namespace ComboSystem
{
    //I really dislike this approach, its gimicky & hacky. Better to probably just have duplicate code/redundancy instead
    public class InterfaceTest
    {
        public interface IModifier
        {
            public bool Apply();
        }
        public interface ISingleUseModifier : IModifier
        {
            public void Init();
        }
    }
    public static class SingleUseComboModifierr
    {
        public static void Init(this InterfaceTest.ISingleUseModifier modifier)
        {
            modifier.Apply();
            modifier.Init();
        }
    }
}