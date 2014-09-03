namespace Avalon.Security.Cryptography.Configuration
{
    using System.Web;

    /// <summary>
    /// The Configuration Manager
    /// </summary>
    internal sealed class ConfigurationManager
    {
        /// <summary>
        /// Initializes a new instance of the ConfigurationManager class.
        /// </summary>
        /// <exception cref="System.Web.HttpRequest">
        /// 620;The Cryptography Manager Section is missing in web.config!
        /// or
        /// 620;The encryption key must be 8 characters long!
        /// or
        /// 550;The encryption vector must be 8 characters long!
        /// </exception>
        public ConfigurationManager()
        {
            var instance = CryptographyManagerSection.Instance;
            if (instance == null)
            {
                throw new HttpException(620, "The Cryptography Manager Section is missing in web.config!");
            }

            this.EncryptionKey = instance.CryptographyKey;

            if (string.IsNullOrWhiteSpace(this.EncryptionKey) || this.EncryptionKey.Length != 8)
            {
                throw new HttpException(620, "The encryption key must be 8 characters long!");
            }

            this.EncryptionVector = instance.CryptographyVector;
            if (string.IsNullOrWhiteSpace(this.EncryptionVector) || this.EncryptionVector.Length != 8)
            {
                throw new HttpException(550, "The encryption vector must be 8 characters long!");
            }

            var iv64 = instance.IV64;
            this.Iv64 = new[]
                                {
                                    iv64.Byte1, 
                                    iv64.Byte2, 
                                    iv64.Byte3, 
                                    iv64.Byte4, 
                                    iv64.Byte5, 
                                    iv64.Byte6, 
                                    iv64.Byte7, 
                                    iv64.Byte8
                                };
        }

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        /// <value>
        /// The encryption key.
        /// </value>
        [NotNull]
        public string EncryptionKey { get; private set; }

        /// <summary>
        /// Gets the encryption vector.
        /// </summary>
        /// <value>
        /// The encryption vector.
        /// </value>
        [NotNull]
        public string EncryptionVector { get; private set; }

        /// <summary>
        /// Gets the iv64.
        /// </summary>
        /// <value>
        /// The iv64.
        /// </value>
        [NotNull]
        public byte[] Iv64 { get; private set; }
    }
}
