namespace Avalon.Security.Cryptography
{
    using System;

    /// <summary>
    /// The json cookie encryption type.
    /// </summary>
    [Flags]
    public enum JsonCookieEncryptionType
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The encrypt des.
        /// </summary>
        // ReSharper disable InconsistentNaming
        EncryptDES = 1,
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// The encrypt base 64.
        /// </summary>
        EncryptBase64 = 2
    }
}