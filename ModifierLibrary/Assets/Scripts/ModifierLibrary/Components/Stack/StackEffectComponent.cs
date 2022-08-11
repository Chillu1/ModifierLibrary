namespace ModifierLibrary
{
	//Could be something to think of when we have multiple EffectComponent with same logic, single value, etc. 
	/*public abstract class StackEffectComponent : EffectComponent, IStackEffectComponent
	{
	    protected double Value { get; set; }
	    
	    protected StackEffectType StackType { get; }

	    protected StackEffectComponent(double value, StackEffectType stackType, ConditionCheckData conditionCheckData = null, bool isRevertible = false) : base(conditionCheckData, isRevertible)
	    {
	        Value = value;
	        StackType = stackType;
	    }

	    public void StackEffect(int stacks, double value)
	    {
	        if (StackType.HasFlag(StackEffectType.Add))
	            Value += value;
	        if (StackType.HasFlag(StackEffectType.AddStacksBased))
	            Value += value * stacks;
	        if (StackType.HasFlag(StackEffectType.Multiply))
	            Value *= value;
	        if (StackType.HasFlag(StackEffectType.MultiplyStacksBased))
	            Value *= value * stacks;

	        
	        if (StackType.HasFlag(StackEffectType.Effect))
	            SimpleEffect();
	    }
	}*/
}