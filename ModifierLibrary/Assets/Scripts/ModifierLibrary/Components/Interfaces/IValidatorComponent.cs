namespace ModifierLibrary
{
	public interface IValidatorComponent<T>
	{
		bool ValidateTarget(T target);
	}

	//class ManaComponent : IValidatorComponent<Mana>
	//{
	//    public bool Validate()
	//    {
	//        throw new System.NotImplementedException();
	//    }
	//}
}