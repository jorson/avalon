<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="fd2dce6e-8123-48a5-ac02-57425e23fff4" namespace="Nd.Web.Security.Cryptography.Configuration" xmlSchemaNamespace="urn:Nd.Web.Security.Cryptography" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
    <externalType name="Byte" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="CryptographyManagerSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="cryptographyManagerSection">
      <attributeProperties>
        <attributeProperty name="CryptographyKey" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="cryptographyKey" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="CryptographyVector" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="cryptographyVector" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="IV64" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="iV64" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/IV64" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="IV64">
      <attributeProperties>
        <attributeProperty name="Byte1" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte1" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte2" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte2" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte3" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte3" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte4" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte4" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte5" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte5" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte6" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte6" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte7" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte7" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
        <attributeProperty name="Byte8" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="byte8" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/fd2dce6e-8123-48a5-ac02-57425e23fff4/Byte" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>