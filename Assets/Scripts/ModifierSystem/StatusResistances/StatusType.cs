using System;
using System.Collections.Generic;
using System.Linq;

namespace ModifierSystem
{
    /// <summary>
    ///     StatusType of which resistance will be applied to ("tags")
    /// </summary>
    [Flags]
    public enum StatusType
    {
        None = 0,
        /// <summary>
        ///     All duration based (time component remove)
        /// </summary>
        Duration = 1,
        /// <summary>
        ///     Stun duration
        /// </summary>
        Stun = 2,
        DoT = 4,
        /// <summary>
        ///     Slow duration
        /// </summary>
        //Slow = 8,
        Resistance = 8,
        /// <summary>
        ///     For elementalData
        /// </summary>
        Element = 16,
        //Heal = 64,

        /// <summary>
        ///     Everything but 0
        /// </summary>
        All = -1
    }

    public static class StatusTypeHelper
    {
        public static readonly StatusType[] StatusTypes = Enum.GetValues(typeof(StatusType)).Cast<StatusType>().ToArray();
    }
}