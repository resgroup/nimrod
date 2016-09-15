using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the description of the enum if it exists,
        /// otherwise returns the name of the enum value.
        /// http://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value
        /// </summary>
        public static string GetDescription(object value) =>
            value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .OfType<DescriptionAttribute>()
                .Select(attribute => attribute.Description)
                .FirstOrDefault() ?? value.ToString();

    }
}
