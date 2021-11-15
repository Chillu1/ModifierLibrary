using System;
using System.Collections.Generic;
using BaseProject;
using BaseProject.Utils;
using JetBrains.Annotations;

namespace ComboSystemComposition
{
    /*
    TODO
    SimpleStatBuff
    StackedDoTDurationModifier

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

        Component recipe:
            DoT

    Encapsulation only based system:

    */

    public class Modifier : IEntity<string>, IEventCopy<Modifier>, ICloneable
    {
        public string Id { get; protected set; }
        [CanBeNull] private IInitComponent InitComponent { get; set; }
        [CanBeNull] private ITimeComponent[] TimeComponents { get; set; }
        //StackComponent
        //RefreshComponent


        public Modifier(string id)
        {
            Id = id;
        }

        public void Init()
        {
            InitComponent?.Init();
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < TimeComponents?.Length; i++)
                TimeComponents[i].Update(deltaTime);
        }

        public void AddComponent(IInitComponent initComponent)
        {
            if (InitComponent != null)
            {
                //logerror
                return;
            }

            InitComponent = initComponent;
        }

        public void AddComponent(IRemoveComponent removeComponent)
        {
            //if (RemoveComponent != null)
            //{
            //    //logerror
            //    return;
            //}

            //RemoveComponent = removeComponent;
        }

        public void AddComponent(ITimeComponent timeComponent)
        {
            if (TimeComponents != null)
            {
                //logerror
                return;
            }

            TimeComponents = new[] { timeComponent };
        }

        public void AddComponent(ITimeComponent[] timeComponents)
        {
            if (TimeComponents != null)
            {
                //logerror
                return;
            }

            TimeComponents = timeComponents;
        }

        public void CopyEvents(Modifier prototype)
        {
            //this.event = prototype.event //or we will need to copy it over properly, with a new reference
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public abstract class Modifier<TDataType> : Modifier
    {
        public TDataType Data { get; }

        protected Modifier(string id, TDataType data) : base(id)
        {
            Data = data;
        }

        // protected Modifier(Modifier<TDataType> other) : base(other)
        // {
        //     Data = other.Data;
        // }
    }
}