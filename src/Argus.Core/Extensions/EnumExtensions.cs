using System;
using System.Linq;

namespace Argus.Extensions
{
    /// <summary>
    ///     Enum Extensions
    /// </summary>
    public static class EnumExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  EnumExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/01/2016
        //      Last Updated:  06/07/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Get's an attribute from an Enum.  This can be used to iterate over the items in an enum.
        ///     Example:
        ///     <code>
        /// foreach (MenuAttribute.MenuGroup group in Enum.GetValues(typeof(MenuAttribute.MenuGroup))) {}
        /// </code>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            string name = Enum.GetName(type, value);

            return type.GetField(name)
                       .GetCustomAttributes(false)
                       .OfType<TAttribute>()
                       .SingleOrDefault();
        }
    }
}