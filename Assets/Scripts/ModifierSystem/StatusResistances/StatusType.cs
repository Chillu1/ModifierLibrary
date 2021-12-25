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
        ///     Stun duration
        /// </summary>
        Stun = 1,
        DoT = 2,
        /// <summary>
        ///     Slow duration
        /// </summary>
        Slow = 4,
        Resistance = 8,
        /// <summary>
        ///     For elementalData
        /// </summary>
        Element = 16,
        Heal = 32,

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