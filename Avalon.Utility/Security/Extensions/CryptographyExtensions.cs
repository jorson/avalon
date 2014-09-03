namespace Avalon.Security.Cryptography
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Text;

	/// <summary>
	/// Cryptography extensions.
	/// </summary>
    public static partial class CryptographyExtensions
    {
        /* ReSharper disable InconsistentNaming */

        /// <summary>
        ///     MD5 hash is a method to obtain a unique "signature" from a string. 
        /// It is very useful in order to store password, but be aware that there 
        /// is no way to get back to the original string, starting from the hash.    
        /// </summary>
        /// <param name="input">The string should be encrypted.</param>
        /// <returns>returns the MD5 hash for any given string</returns>
        [NotNull]
        public static string EncryptAsMd5Hash([NotNull] this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                var builder = new StringBuilder();

                using (var stream = new MD5CryptoServiceProvider())
                {
                    byte[] data = stream.ComputeHash(Encoding.UTF8.GetBytes(input));

                    foreach (byte character in data)
                    {
                        builder.Append(character.ToString("x2"));
                    }
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        ///     Encrypts a string using DES algorithm.
        /// </summary>
        /// <param name="input">The string should be encrypted.</param>
        /// <returns>The encrypted string.</returns>
        [NotNull]
        public static string EncryptAsDES([NotNull] this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                byte[] plainText = Encoding.UTF8.GetBytes(input);
                byte[] key = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionKey);
                byte[] iv = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionVector); // "init vec is big."
                byte[] cipherText = CryptographyManager.Encrypt(plainText, key, iv);

                return Convert.ToBase64String(cipherText);
            }

            return string.Empty;
        }

        /// <summary>
        /// Encrypts the DES.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The encrypted bytes</returns>
        [CanBeNull]
        public static byte[] EncryptAsDES([NotNull] this byte[] input)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (input != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                byte[] key = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionKey);
                byte[] iv = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionVector); // "init vec is big."
                return CryptographyManager.Encrypt(input, key, iv);
            }

            return null;
        }

        /// <summary>
        ///     Decrypts a string. The 8-bit string for decryption. Exactly 8 Char long key
        /// </summary>
        /// <param name="input">The encrypted string to be decrypted.</param>
        /// <returns>The plain text.</returns>
        [NotNull]
        public static string DecryptAsDES([NotNull] this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                byte[] cipherText = Convert.FromBase64String(input);

                return DecryptAsDES(cipherText);
            }

            return string.Empty;
        }

        /// <summary>
        ///     Decrypts a string. The 8-bit string for decryption. Exactly 8 Char long key
        /// </summary>
        /// <param name="input">The encrypted byte array to be decrypted.</param>
        /// <returns>The plain text in string format.</returns>
        [NotNull]
        public static string DecryptAsDES([NotNull] this byte[] input)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (input != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                // The IV property is set to a new random value whenever you create a new instance of one of the SymmetricAlgorithm classes 
                byte[] iv = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionVector);
                byte[] key = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionKey);

                byte[] plainText = CryptographyManager.Decrypt(input, key, iv);

                return Encoding.UTF8.GetString(plainText);
            }

            return string.Empty;
        }

        /// <summary>
        ///     Decrypts a string. The 8-bit string for decryption. Exactly 8 Char long key
        /// </summary>
        /// <param name="input">The encrypted byte array to be decrypted.</param>
        /// <returns>The plain text in string format.</returns>
        [CanBeNull]
        public static byte[] DecryptAsDESBytes([NotNull] this byte[] input)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (input != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                // The IV property is set to a new random value whenever you create a new instance of one of the SymmetricAlgorithm classes 
                byte[] iv = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionVector);
                byte[] key = Encoding.UTF8.GetBytes(CryptographyManager.Manager.EncryptionKey);

                return CryptographyManager.Decrypt(input, key, iv);
            }

            return null;
        }

        /* ReSharper restore InconsistentNaming */
    }
}
