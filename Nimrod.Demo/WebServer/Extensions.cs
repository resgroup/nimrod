using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Demo
{
    public static class Extensions
    {
        public static string ToJson(this object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                settings.Converters.Add(new IsoDateTimeConverter() { DateTimeStyles = DateTimeStyles.AdjustToUniversal });
                var serializer = JsonSerializer.Create(settings);
                serializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
    }
}
