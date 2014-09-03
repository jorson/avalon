// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.Base64AndBytes.cs" company="genuine">
//     Copyright (c) .NET Minion Team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//  Author:     J.Baltika
//  Date:       07/24/2010
// --------------------------------------------------------------------------------------------------------------------
namespace Avalon.Security.Cryptography
{
    using System;
    using System.Text;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Extension methods partial class.")]
    internal static partial class StringExtensions
    {
        /// <summary>
        /// Encodes the input value to a Base64 string using the supplied encoding.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>
        /// The Base 64 encoded string
        /// </returns>
        [NotNull]
        public static string EncodeAsBase64([NotNull] this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var bytes = Encoding.UTF8.GetBytes(value);

                return Convert.ToBase64String(bytes);
            }

            return string.Empty;
        }

        /// <summary>
        /// Decodes a Base 64 encoded value to a string using the supplied encoding.
        /// </summary>
        /// <param name="encodedValue">The Base 64 encoded value.</param>
        /// <returns>
        /// The decoded string
        /// </returns>
        [NotNull]
        public static string DecodeAsBase64([NotNull] this string encodedValue)
        {
            if (!string.IsNullOrWhiteSpace(encodedValue))
            {
                var bytes = Convert.FromBase64String(encodedValue);

                return Encoding.UTF8.GetString(bytes);
            }

            return string.Empty;
        }
    }
}
