TODO RN:    
> Linger makes it so "fast spells" dont do their effect, because modifier is already present  
>    Solution: make a class for status effects in Being, instead of having a linger for combos.
>              But then we can't combo based of present modifier IDs
>    Solution 2: //If we didnt stack or refresh, then apply internal modifier effect again? Any issues? We could limit this with a flag/component
                if(!stacked && !refreshed)
                    internalModifier.Init();//Problem comes here, since the effect might not actually be in Init()

THIS
Start removing inheritance, and change to composition
Unittest:
    Value & intensity updates with time (10 linger = 1 second, 100 linger = 5 sec, 1000 linger = 20)±
    First more proper Stats setup(fix inheritance/design), then: ComboModifier Stat based
    Stat change removal after duration/remove effect

> AspectOfTheCat & Infection
ComboMod ConditionalRemove, timer starts ticking down after condition isnt met anymore? (Might be better to just have a time remove, and comboMod is added again later, or we refresh the duration when it tries to be added again)
> Cooldown

Combo design choices:
    Should ComboModifier be applied again?/What happens when there's a duplicate?
    ComboModifier multiple recipes.
    Statbased comboModifier, when health is bigger than 10k, "massive" comboModifier

When should we check for combo modifiers to add?
    OnAddModifier
    & every 1 second on adding elementalData?

When should elemental damage be applied? DealDamage?
When should elementalData be applied, that's not damage? OnAddModifier?

Element example, fire:
    effectValue = general strongness, ligeringValue = for how long it stays
    Fireball = 20 effectValue, ligeringValue = 10  
    Flamethrower = 10 effectValue, ligeringValue = 20
    Firestorm = 60 effectValue, ligeringValue = 20
    Meteor = 150 effectValue, ligeringValue = 50
    Fire of a small star = 1000 effectValue, ligeringValue = 100
    Fire of middle of a star = 5000 effectValue, ligeringValue = 300

ComboModifiers:
    AspectOfTheCat (IDs)
    Intensity Burning = 5
    Intensity X = 5, Y = 5
    

ComboModifiers concept
    What does combomodifier need that's different from modifier?
    Should ComboModifier be a separate class in general? Since it might not have enough of the same mechanics?
        Activation conditions
        Cooldown (so the combomodifier can only be triggered X often (aka damage isn't spammed by mixing fire & preasured gass to make an explosion every attack))
    Combo examples:
        Explosion (fire & preasured gass)
ComboCondition backend:
    OnAddModifier check for combinations
    Possible things to check for:
        Enough elemental attacks lingering/on being (elemental value that goes down over time?)
        Specific Modifiers (IDs)

RefreshComponent is also fairly complex, cuz it's still tricky on how to make it proper, should refresh only refresh timer duration? If so then it's useless
    since we can refresh directly through timerComponent (but then we'll need to know which one to refresh, so maybe not?)
RefreshComponent:
    refresh duration
    & increase duration
    & incrase effect?

StackComponent:
What can a stack do?
    Increase numbers (damage, speed, TimeComponent.duration)
    Trigger an effect on X stacks

Whats the issue?
We're using the prototype pattern, so we need to clone every element of the modifier & being.
We also need to clone StackComponent, and the behaviour it should have on stack.
That cloned behaviour should now point to the new cloned effect, we can't use delegates, unless we pass the object directly into them & don't ref anything "new"
So, save behaviour, but without inheritance & delegates.

Now, how does one define complex behaviour? We have to hardcode it in the class

Pass in the reference, and use it as action

MetaEffect, that changes an IEffectComponent

Hey everyone, design problem here.

I want a very "generic"/open delegate like "Action". So it can be used in a lot of ways, like incrementing X and Y.
But also doing X effect on Y count. So limiting the behaviour is very not ideal, aka hardcoding the cases.
Now, the problem is, the prototype pattern. Im using it with deep cloning the entire object.
This is fine, until we try to clone the "Action", we can clone it to be the same. But then:
1. Our target will be wrong (old).
2. I don't want to hardcore the Action data in the class, since it can be a lot of different things.

Another possibility is that.
I might be going about this the wrong way. Maybe there's an elegant way of doing this open-ended behaviour.
Without using delegates, and a lot of hard-coding the behaviours in the class.


_Refactor "unitType"_  
    Self, Ally, Enemy.  Dynamically added based on what unit type you are?
Basic Unit tests of components & mechanics
    _damageData[0].BaseDamage is not being used to damage enemy
    Generic methods? Increase damage on stack, etc
UniqueId per new component?
ConditionalApply (OnAttack, OnDeath, etc)

Heal -> Lifesteal (heal based on damage dealt)    
Unit test of all mechanics & components  
  
TODO:  
ApplierModifier  
Figure out order of operations:  
Attack  
GetApplierMods  
SetTarget of ApplierMods  
Apply (effect) applier Mods  
Set target before applying (& Effect())  
Later:  
ConditionalApply, OnAttack, OnCast, OnDeath  
SimpleAttackBuff  
AddTarget (when?) (init & apply need it)  
Remove after linger time from modController  
SimpleStatBuff  
StackedDoTDurationModifier  
Unity UI for controlling values, timers, etc
  
Basic example inheritance:  
DamageAttackModifier:  
InitUseModifier (Apply on init)  
-> SingleUseModifier (Gets removed after linger time)  
-> DamageAttackModifier (Actual effect) Target.DealDamage(damage)  
  
  
Component based system (mixing components to a new modifier, like a recipe):  
Components needed:  
https://www.reddit.com/r/gamedev/comments/1bm5xs/programmers_how_dowould_you_implement_a/c9848mc/  
Effect  
Target (makes sure Target is valid)  
Component types:  
ApplyComponent  
Simple apply, no rules, just call effect  
Conditional apply, when effect is triggered & a conditional value is true  
DurationComponent  
Remove after duration  
Effect after duration?  
StackComponent  
RefreshComponent?  
RemoveComponent  
TimeComponent  
  
        Component recipe:  
            DoT  
  
  
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
  
    InitComponent -> ApplyComponent -> EffectComponent  
    TimeComponent -> ApplyComponent -> EffectComponent  
    TimeComponent -> RemoveComponent  
  
     Structure  
            Dump components not knowing what they do (time(duration), single use, time(interval))  
            EffectComponents  
               Have an effect: stun, statchange, deal damage  
            ApplyComponents  
               Trigger effect: after duration, on interval, condition, in apply  
            TimeComponents  
               Trigger effect: after duration/ on interval  
    */  
  
    /*  
     ModifierSet (Aspect of the cat)  
        AddStatModifier (Speed buff)  
        AddStatModifier (AttackSpeed buff)  
        DurationModifier (RemoveComponent, removed ModifierSet)  
     */  
  
    //RefreshedDoTIntervalDurationModifier  
    /*  
      Workings:  
        Added to collection/entity instantly  
        After X Interval, does Y Damage  
        After Z Duration, gets removed from collection/entity  
        if  modifier added again to collection/entity  
            refresh modifier _timer  
    */  
  
Modifier ideas:  
X can be: damage, speed, stat, health, mana, etc  
  
Different types of lifesteal:  
Based on damage dealt (done)  
Based on damage dealt before resistance reduction (done)  
Based on enemy's current/max health %  
  
  
When lower than X health do Y  
Heal damage instead of getting damaged for 3 seconds  
Stack More damage every instance  
More damage every missing X (mana, health, lower level than hero)  
Take control over enemy creep for X seconds  
When attacked do X  
When attacked by X damage do Y, then reset damage count  
Cant attack, cant be attacked. (Nightmare)  
Reduced regen (healing, mana, etc)  
More float X or X% with missing health/mana/etc (huskar)  
Thorns  
On kill enemies "Explode" dealing damage (can be comboed with, explode, dealing hp as dmg?) Would be a cool combo modifier  
The more damage you deal to yourself, the stronger your attacks are  
Every X attack do Y  
Every X cast do Y  
For every nearby enemy, do more X  
More X for every missing hp/mana on enemies/allies  
Backstab  
Bristleback  
Remove 30% of enemies health, but do the same to your hero  
IO link (healing, mana)  
Asborb X damage  
Counterattack % on attacked  
Lifesteal  
Lifesteal %current health  
Reduce forwards physical attacks (mars)  
More X at day/night  
For every kill gain X permanent stat  
Fast attacks, 70% lower damage  
Physical damage block  
Lower enemies physical damage %  
Reduce enemy stat(s) for X seconds  
Linked beings, if one dies, so does the other. Buff each other  
Glaive bounces  
Mana shield  
Health is mana, mana is health  
Invis?  
Morph into enemy, gain 1-2 of his spells (might be bad)  
Mana burn  
Reflect one damage instance  
Disarm  
Crit  
On attack X chance to spawn illu  
Link (razor)  
Souls (SF)  
Steal attributes/stats for X duration on X  
Spectre E  
Refractions (TA)  
Every hit deal more damage (Ursa)  
Lower regen health/mana/etc by 70-90% with X duration  
More damage based on distance  
Binds two targets, every target spell gets applied to both.  
Warlock binds Q  
  
  
Combo mod ideas:  
Remove all base resistances & armor  
Stop "time" for X seconds  
When close to dying, shadow grave, duration based on skill cooldown.