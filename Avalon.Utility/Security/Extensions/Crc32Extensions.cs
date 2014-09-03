namespace Avalon.Security.Cryptography
{
    using System;
    using System.Diagnostics;
    using System.Text;

	/// <summary>
	/// Crc32 extensions.
	/// </summary>
    [DebuggerStepThrough, CLSCompliant(false)]
    public static partial class Crc32Extensions
    {
        /// <summary>
        ///     Computes the CRC32 value.
        /// </summary>
        /// <param name="data">The data in byte format.</param>
        /// <returns>The CRC32 value</returns>
        [NotNull, CLSCompliant(false)]
        public static string ComputeCrc32([NotNull] this byte[] data)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (data != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                using (var crc32 = new Crc32())
                {
                    var builder = new StringBuilder();

                    foreach (byte b in crc32.ComputeHash(data))
                    {
                        builder.Append(b.ToString("x2"));
                    }

                    return builder.ToString().ToLowerInvariant();
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Computes the CRC32 value.
        /// </summary>
        /// <param name="data">The data in string format.</param>
        /// <returns>The CRC32 value</returns>
        [NotNull, CLSCompliant(false)]
        public static string ComputeCrc32([NotNull] this string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                if (bytes != null)
                {
                    return bytes.ComputeCrc32();
                }
            }

            return string.Empty;
        }
    }
}
