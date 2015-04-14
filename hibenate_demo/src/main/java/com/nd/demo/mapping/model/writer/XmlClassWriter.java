package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.classbased.ClassMapping;
import org.dom4j.dom.DOMDocument;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-04-14
 */
public class XmlClassWriter extends XmlClassWriterBase implements XmlWriter<ClassMapping> {

    private final XmlWriterServiceLocator serviceLocator;

    public XmlClassWriter(XmlWriterServiceLocator serviceLocator) {
        super(serviceLocator);
        this.serviceLocator = serviceLocator;
    }

    @Override
    public Document write(ClassMapping mappingModel) {
        document = null;
        mappingModel.acceptVisitor(this);
        return document;
    }

    @Override
    public void processClass(ClassMapping classMapping) {
        document = new DOMDocument();

        Element classElement = document.createElement("class");
        classElement.setAttribute("xmlns", "urn:nhibernate-mapping-2.2");

        if(classMapping.isSpecified("BatchSize")) {
            classElement.setAttribute("batch-size", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("DiscriminatorValue")) {
            classElement.setAttribute("discriminator-value", classMapping.getDiscriminatorValue().toString());
        }
        if(classMapping.isSpecified("DynamicInsert")) {
            classElement.setAttribute("dynamic-insert", String.valueOf(classMapping.getDynamicInsert()));
        }
        if(classMapping.isSpecified("DynamicUpdate")) {
            classElement.setAttribute("dynamic-update", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Lazy")) {
            classElement.setAttribute("lazy", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Schema")) {
            classElement.setAttribute("schema", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Mutable")) {
            classElement.setAttribute("mutable", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Polymorphism")) {
            classElement.setAttribute("polymorphism", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Persister")) {
            classElement.setAttribute("persister", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Where")) {
            classElement.setAttribute("where", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("OptimisticLock")) {
            classElement.setAttribute("optimistic-lock", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Check")) {
            classElement.setAttribute("check", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Name")) {
            classElement.setAttribute("name", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("TableName")) {
            classElement.setAttribute("table", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Proxy")) {
            classElement.setAttribute("proxy", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("SelectBeforeUpdate")) {
            classElement.setAttribute("select-before-update", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Abstract")) {
            classElement.setAttribute("abstract", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("Subselect")) {
            classElement.setAttribute("subselect", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("SchemaAction")) {
            classElement.setAttribute("schema-action", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("EntityName")) {
            classElement.setAttribute("entity-name", classMapping.getEntityName());
        }
    }
}
