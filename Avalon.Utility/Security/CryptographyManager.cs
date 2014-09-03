namespace Avalon.Security.Cryptography
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;
    using Avalon.Security.Cryptography.Configuration;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    internal static partial class CryptographyManager
    {
        /// <summary>
        /// The settings
        /// </summary>
        public static readonly ConfigurationManager Manager = new ConfigurationManager();

        /// <summary>
        ///     Encrypts the specified bytes data.
        /// </summary>
        /// <param name="byteData">The data as bytes.</param>
        /// <param name="byteKey">The key as bytes.</param>
        /// <param name="initVec">The IV property is set to a new random value whenever you create a new instance of one of the SymmetricAlgorithm classes</param>
        /// <returns>Encrypted byte stream</returns>
        [NotNull, SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "One rule contradicts another")]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "One rule contradicts another")]
        internal static byte[] Encrypt([NotNull] byte[] byteData, [NotNull] byte[] byteKey, [NotNull] byte[] initVec)
        {
            using (var memoryStream = new MemoryStream(1024))
            {
                using (var des = new DESCryptoServiceProvider { Mode = CipherMode.CBC, Key = byteKey, IV = initVec })
                {
                    using (ICryptoTransform encryptor = des.CreateEncryptor())
                    {
                        using (var stream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            stream.Write(byteData, 0, byteData.Length);
                            stream.FlushFinalBlock();

                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Decrypts the specified bytes data.
        /// </summary>
        /// <param name="byteData">The data as bytes.</param>
        /// <param name="byteKey">The key as bytes.</param>
        /// <param name="initVec">The IV property is set to a new random value whenever you create a new instance of one of the SymmetricAlgorithm classes</param>
        /// <returns>Decrypted string</returns>
        [NotNull]
        internal static byte[] Decrypt([NotNull] byte[] byteData, [NotNull] byte[] byteKey, [NotNull] byte[] initVec)
        {
            using (var memoryStream = new MemoryStream(1024))
            {
                using (var provider = new DESCryptoServiceProvider())
                {
                    provider.Mode = CipherMode.CBC;
                    provider.Key = byteKey;
                    provider.IV = initVec;

                    using (ICryptoTransform transform = provider.CreateDecryptor())
                    {
                        var stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);

                        stream.Write(byteData, 0, byteData.Length);
                        stream.FlushFinalBlock();
                    }

                    return memoryStream.ToArray();
                }
            }
        }
    }
}
