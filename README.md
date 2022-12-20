# What is this?

It's a buff/debuff/modifier library that's an old previous version of the [ModiBuff library](https://github.com/Chillu1/ModiBuff).    
It has **A LOT** of features, but isn't optimized

For all modifiers, and their recipes go
to [Modifier Prototypes](https://github.com:Chillu1/ModifierSystem/blob/master/ModifierLibrary/Assets/Scripts/ModifierLibrary/ModifierPrototypes.cs#L126-L1034)

I **HIGHLY** recommend using the new version. And only use this one in case of research or if you need a lot of features and don't care
about optimizations/GC.

An ECS version will also be released soon [here](https://github.com/Chillu1/ModiBuffEcs)

I will not be updating this version anymore. But it does have some interesting mechanic ideas.

# Terms

| Term             | Explanation                                                                                                                        |
|------------------|------------------------------------------------------------------------------------------------------------------------------------|
| StatusResistance | Resistance for any kind of status: elemental damage, elemental data, DoT, duration, negative debuffs, etc.                         |
| StatusTag        | Tag for all types of statuses: elemental damage, elemental data, DoT, duration, negative debuffs, etc.                             |
| DamageData       | Holds damage values, DamageType, and ElementalData                                                                                 |
| ElementData      | Holds type and effectData of elements. Ex: Fireball, Fire, 10 effect, 10 linger                                                    |
| DamageResistance | DamageType and ElementalType resistances: Physical, Magical, Pure, and, Acid, Cold, Fire.                                          |
| **ModifierSystem**|                                                                                                                                   |
| Modifier         | Buff/Debuff on beings, can do anything, slow, over time/delayed stun, change stats, deal damage, resurrect                         |
| ModifierApplier  | A special modifier that applies another modifier on someone by either cast or attack                                               |
| ComboModifier    | Special Modifier that is activated on specific conditions (recipes), these can be: specific modifiers (ID), ElementalData or Stats |
| ComboRecipe      | Recipe (condition) for a ComboModifier to be added, possible conditions: specific modifiers (ID), ElementalData or Stats           |

# Rules/Ways of doing stuff

Only components that should be initiated directly into modifier:

* init
* timeEffect
* stack
* refresh

## Creating a new modifier

![ModifierFlowChart](Docs/ModifierFlowChart.png "Modifier Flow Chart")

## What interacts with what, component relations?

Blue = Needed  
Green = One Needed (holder of Effect), darker green = more common  
Red = Different types of Effect  
White = Optional  
Blank arrow = Optional  
Double arrow = Params

![Components](Docs/ModifierComponents.png "Components")

Must have: Target, Effect    
One of: Init/Time/Apply/Stack  
Optional: CleanUp, Stack, Refresh

Direct Order:  
Modifier, Data,  
Target, Effect, Apply, Init, CleanUp, Remove, Time, Stack, Refresh

**Ideal order**:  
Modifier, Data.  
Target, Effect, Apply, CleanUp, Remove, TimeRemove.  
Init, TimeEffect, Stack, Refresh

Making a modifier decision flow chart:

* Duration based
	* TimeComponentEffect
	* Refreshable duration
		* RefreshComponent
* Finite duration
	* TimeComponentRemove
* OnStart/Init
	* Apply & Init
* StackableEffect
	* StackComponent
	  ConditonBased

Applier

# Design questions

* EffectStacking, with Value changing. Ex. DamageRes, stacking. Makes it so we can't easily revert if atter the buff is gone. Aka we can't
  feed stacks values to revertEffect in a clean way
* ApplyAttackModifiers iteration, needs a copy, beceause attacking might lead to a new modifier being added. Have a separate list of keys of
  appliers?
  Or make new logic that ads the modifier after we have fully iterated through it (might not be ideal).
* We have to deep clone effect in EffectPropertyInfo so it can be reused without multiple targets holding the same effect, and overwriting
  each other
  Not ideal, best to have some sort of data-only in Properties, and then make a new effect class here.
  Problem with that, is that we'll need to make a new effect using reflection?
  Or a centralized method to make new effects, that needs to be updated for each new effect... Both, bad solutions...
* Either all buffs need to be refreshable, or we need a special check if something is a buff effect, to not apply it again on init
* First being events have priority rn over all the other ones, so their calls will take over other events counters, ex, AttackEvent heal 1
  health, will take over next event that is ex. HitEvent heal 100 health
	* We could maybe set some sort of priority to all events? Or give everyone a fair shot/fair deal instead of limiting every event?
* How to check for ConditionCheck applies? Through a special timeComponent logic? That checks every X, if true, effect. If effect passed,
  wait. Better than OnDamagedEvent and such?
* In EffectComponent, should be pass _targetComponent.Owner as acter instead of null? We might want to check owner, when its a normal
  non-condition effect? At some point.
* CleanUpComponent only needed when we have remove? Automated with apply otherwise?
* What more mechanics can refreshComponent have?
* Single/X use condition modifiers, how? RemoveComponent can hold X stacks, lowering down to 0, then actual effect is triggered
* Should CleanUp remove buffs/upgrades, ex. on kill. We would need to record the amount, or store it in a different way
* Order of BaseBeing.Update() and ModifierController.Update() matters for StatusEffect
* More efficient way of saving timer data related to enum/id (not dict? but array of some sort?)
* OnStatChange ComboModifierRecipe check, how? We don't have a generic function for all stat changes where we could check for combos
* Removing all modifiers from ModifierController wont remove the parmanent buffs & debuffs, make a function that does the opposite for
  these?
* What to do with Positive/Negative status tags, how to automate positive? Check for positive value on stats, buffs, etc?
* Should appliers be allowed for conditional? Most probably yes, like applying poison modifier on getting hit
* We might come into trouble with multiple target components, since rn we rely on having only one in modifier

## Design solutions

* Recursion problem, when a condition event is triggered, there's a chance it will be triggered again by the same call. Disabling them fully
  is kinda uncool, since it makes some nice interactions impossible
	* BaseBeingEventController. Whenever we go through code controller tells us when to trigger event

# Modifier

Process (lifecycle):  
Modifier tries to be added to entity collection  
We check for duplicates, refresh, stack, etc.  
When added, apply may be called (ex. on init buff)  
Update all updatable components in collection (ex. TimeComponent)  
Apply components check for validity of apply  
if passed: Trigger EffectComponents  
Either triggers an effect, or removes modifier after duration passed

Technical lifecycle:  
ModifierManager.AddModifier(modifier)  
ModifierManager.CheckDuplicate(modifier)  
Stack or Refresh  
If targetself, setTarget(self)  
modifier.Init()  
May modifier.Apply()  
Update TimeComponent  
May modifier.Apply()  
After linger/duration remove ModifierManage.RemoveModifier(modifier)

Figure out order of operations:  
Attack  
GetApplierMods  
SetTarget of ApplierMods  
Apply (effect) applier Mods  
Set target before applying (& Effect())

Linger makes it so "fast spells" dont do their effect, because modifier is already present  
Solution: make a class for status effects in Being, instead of having a linger for combos.
But then we can't combo based of present modifier IDs
Solution 2: //If we didnt stack or refresh, then apply internal modifier effect again? Any issues? We could limit this with a flag/component
if(!stacked && !refreshed)
internalModifier.Init();//Problem comes here, since the effect might not actually be in Init()

## Modifier Design Process

Designinger a modifier: Strong direct specific upside rolled to a significant amount and a strong downside that should be always somehow
related to the upside
, ex: 1.5x attack speed, hp lowered by 30-50%.
, bad ex: 1.1x attack speed, spell casting speed lowered by 10% (we don't care for spell cast speed if we're attacks-only build, aka
straight buff)
Modifier ideas:  
X can be: damage, speed, stat, health, mana, etc

## ComboModifier

How do cooldowns?
Dict of added comboMods in ModController, ticking downwith time (updating values every second)
Remove ID from dict/hashset on 0

When should we check for combo modifiers to add?  
OnAddModifier  
& every 1 second & on adding elementalData?  
& every 1 second & on stat change?

ComboMod ConditionalRemove, timer starts ticking down after condition isnt met anymore? (Might be better to just have a time remove, and
comboMod is added again later, or we refresh the duration when it tries to be added again)

Should ComboModifier be applied again?/What happens when there's a duplicate?  
ComboModifier multiple recipes.  
Statbased comboModifier, when health is bigger than 10k, "massive" comboModifier (best to make another condition, so it's more unique, and
not every enemy after level X has it...)

ComboModifiers concept  
What does combomodifier need that's different from modifier?  
Should ComboModifier be a separate class in general? Since it might not have enough of the same mechanics?  
Activation conditions  
Cooldown (so the combomodifier can only be triggered X often (aka damage isn't spammed by mixing fire & preasured gass to make an explosion
every attack))  
Combo examples:  
Explosion (fire & preasured gass)  
ComboCondition backend:  
OnAddModifier check for combinations  
Possible things to check for:  
Enough elemental attacks lingering/on being (elemental value that goes down over time?)  
Specific Modifiers (IDs)

## Components

Component based system (mixing components to a new modifier, like a recipe):  
Components needed:  
Effect  
Target (makes sure Target(s) is valid)  
Component types (-Component):

* Init
* Apply
	* Simple apply, no rules, just call effect
	* Conditional apply, when effect is triggered & a conditional value is true
* Time
* Stack
* Refresh
* Remove
* CleanUp  
  Non-Technical:
* Graphics
* Sound
* ParticleEffect

### StackComponent

What can a stack do?

* Increase numbers (damage, speed, TimeComponent.duration)
* Trigger an effect on X stacks
* Trigger an effect every X stacks
* X stacks amount * Y Value (ex. damage)

### RefreshComponent

RefreshComponent is also fairly complex, cuz it's still tricky on how to make it proper, should refresh only refresh timer duration? If so
then it's useless  
since we can refresh directly through timerComponent (but then we'll need to know which one to refresh, so maybe not?)  
RefreshComponent:  
refresh duration  
& increase duration  
& trigger effect