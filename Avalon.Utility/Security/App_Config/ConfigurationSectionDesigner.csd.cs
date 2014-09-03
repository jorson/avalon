namespace Avalon.Security.Cryptography.Configuration
{
    
    
    /// <summary>
    /// The CryptographyManagerSection Configuration Section.
    /// </summary>
    public partial class CryptographyManagerSection : global::System.Configuration.ConfigurationSection
    {
        
        #region Singleton Instance
        /// <summary>
        /// The XML name of the CryptographyManagerSection Configuration Section.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string CryptographyManagerSectionSectionName = "cryptographyManagerSection";
        
        /// <summary>
        /// Gets the CryptographyManagerSection instance.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public static global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection Instance
        {
            get
            {
                return ((global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection)(global::System.Configuration.ConfigurationManager.GetSection(global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyManagerSectionSectionName)));
            }
        }
        #endregion
        
        #region Xmlns Property
        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string XmlnsPropertyName = "xmlns";
        
        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.XmlnsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.XmlnsPropertyName]));
            }
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region CryptographyKey Property
        /// <summary>
        /// The XML name of the <see cref="CryptographyKey"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string CryptographyKeyPropertyName = "cryptographyKey";
        
        /// <summary>
        /// Gets or sets the CryptographyKey.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The CryptographyKey.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyKeyPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual string CryptographyKey
        {
            get
            {
                return ((string)(base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyKeyPropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyKeyPropertyName] = value;
            }
        }
        #endregion
        
        #region CryptographyVector Property
        /// <summary>
        /// The XML name of the <see cref="CryptographyVector"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string CryptographyVectorPropertyName = "cryptographyVector";
        
        /// <summary>
        /// Gets or sets the CryptographyVector.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The CryptographyVector.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyVectorPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual string CryptographyVector
        {
            get
            {
                return ((string)(base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyVectorPropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.CryptographyVectorPropertyName] = value;
            }
        }
        #endregion
        
        #region IV64 Property
        /// <summary>
        /// The XML name of the <see cref="IV64"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string IV64PropertyName = "iV64";
        
        /// <summary>
        /// Gets or sets the IV64.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The IV64.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.IV64PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual global::Avalon.Security.Cryptography.Configuration.IV64 IV64
        {
            get
            {
                return ((global::Avalon.Security.Cryptography.Configuration.IV64)(base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.IV64PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.CryptographyManagerSection.IV64PropertyName] = value;
            }
        }
        #endregion
    }
}
namespace Avalon.Security.Cryptography.Configuration
{
    
    
    /// <summary>
    /// The IV64 Configuration Element.
    /// </summary>
    public partial class IV64 : global::System.Configuration.ConfigurationElement
    {
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region Byte1 Property
        /// <summary>
        /// The XML name of the <see cref="Byte1"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte1PropertyName = "byte1";
        
        /// <summary>
        /// Gets or sets the Byte1.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte1.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte1PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte1
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte1PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte1PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte2 Property
        /// <summary>
        /// The XML name of the <see cref="Byte2"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte2PropertyName = "byte2";
        
        /// <summary>
        /// Gets or sets the Byte2.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte2.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte2PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte2
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte2PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte2PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte3 Property
        /// <summary>
        /// The XML name of the <see cref="Byte3"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte3PropertyName = "byte3";
        
        /// <summary>
        /// Gets or sets the Byte3.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte3.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte3PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte3
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte3PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte3PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte4 Property
        /// <summary>
        /// The XML name of the <see cref="Byte4"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte4PropertyName = "byte4";
        
        /// <summary>
        /// Gets or sets the Byte4.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte4.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte4PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte4
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte4PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte4PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte5 Property
        /// <summary>
        /// The XML name of the <see cref="Byte5"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte5PropertyName = "byte5";
        
        /// <summary>
        /// Gets or sets the Byte5.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte5.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte5PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte5
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte5PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte5PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte6 Property
        /// <summary>
        /// The XML name of the <see cref="Byte6"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte6PropertyName = "byte6";
        
        /// <summary>
        /// Gets or sets the Byte6.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte6.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte6PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte6
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte6PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte6PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte7 Property
        /// <summary>
        /// The XML name of the <see cref="Byte7"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte7PropertyName = "byte7";
        
        /// <summary>
        /// Gets or sets the Byte7.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte7.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte7PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte7
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte7PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte7PropertyName] = value;
            }
        }
        #endregion
        
        #region Byte8 Property
        /// <summary>
        /// The XML name of the <see cref="Byte8"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        internal const string Byte8PropertyName = "byte8";
        
        /// <summary>
        /// Gets or sets the Byte8.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.0")]
        [global::System.ComponentModel.DescriptionAttribute("The Byte8.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Avalon.Security.Cryptography.Configuration.IV64.Byte8PropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual byte Byte8
        {
            get
            {
                return ((byte)(base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte8PropertyName]));
            }
            set
            {
                base[global::Avalon.Security.Cryptography.Configuration.IV64.Byte8PropertyName] = value;
            }
        }
        #endregion
    }
}
