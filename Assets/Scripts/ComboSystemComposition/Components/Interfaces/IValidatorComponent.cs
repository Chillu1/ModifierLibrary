namespace ComboSystemComposition
{
    public interface IValidatorComponent<T>
    {
        bool Validate(T target);
    }

    //class ManaComponent : IValidatorComponent<Mana>
    //{
    //    public bool Validate()
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}