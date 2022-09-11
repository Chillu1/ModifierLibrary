namespace ModifierLibraryLite
{
	public class DamageEffect : IEffect
	{
		private float _damage;

		public void Effect(IUnit target)
		{
			target.TakeDamage(_damage);
		}
	}
}