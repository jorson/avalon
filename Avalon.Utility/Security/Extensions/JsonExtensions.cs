namespace Avalon.Security.Cryptography
{
    using System.Diagnostics;

    /// <summary>
    ///     The JSON extension methods
    /// </summary>
    [DebuggerStepThrough]
    internal static partial class JsonExtensions
    {
        /// <summary>
        /// Uses JSON.NET instead Microsoft ASP.NET JSON. Deserializes JSON-formatted data into ECMAScript (JavaScript) types
        /// </summary>
        /// <typeparam name="TObject">Object Type</typeparam>
        /// <param name="json">Serialized Object as string</param>
        /// <returns>
        /// deserialized object of type TObject
        /// </returns>
        [CanBeNull]
        public static TObject JsonDecode<TObject>([CanBeNull]this string json) where TObject : class
        {
            if (!string.IsNullOrEmpty(json))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TObject>(json);
            }

            return default(TObject);
        }

        /// <summary>
        /// Uses Microsoft ASP.NET JSON instead of JSON.NET. Serialize Generic Object to the JavaScript Notation Object (JSON)
        /// </summary>
        /// <param name="value">The object value.</param>
        /// <returns>
        /// Serialized JSON string
        /// </returns>
        [NotNull]
        public static string JsonEncode([CanBeNull]this object value)
        {
            if (value != null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }

            return string.Empty;
        }
    }
}
