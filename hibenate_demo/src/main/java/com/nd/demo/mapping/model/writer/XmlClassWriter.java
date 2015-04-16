package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.identity.IdentityMapping;
import org.dom4j.Document;
import org.dom4j.DocumentFactory;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.dom4j.dom.DOMDocumentFactory;

import static com.nd.demo.mapping.model.writer.XmlExtensions.withAttr;

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

        document = DocumentHelper.createDocument();
        Element classElement = document.addElement("class");
        withAttr(classElement, "xmlns", "urn:nhibernate-mapping-2.2");

        if(classMapping.isSpecified("BatchSize")) {
            withAttr(classElement, "batch-size", String.valueOf(classMapping.getBatchSize()));
        }
        if(classMapping.isSpecified("DiscriminatorValue")) {
            withAttr(classElement, "discriminator-value", classMapping.getDiscriminatorValue().toString());
        }
        if(classMapping.isSpecified("DynamicInsert")) {
            withAttr(classElement, "dynamic-insert", String.valueOf(classMapping.getDynamicInsert()));
        }
        if(classMapping.isSpecified("DynamicUpdate")) {
            withAttr(classElement, "dynamic-update", String.valueOf(classMapping.getDynamicUpdate()));
        }
        if(classMapping.isSpecified("Lazy")) {
            withAttr(classElement, "lazy", String.valueOf(classMapping.getLazy()));
        }
        if(classMapping.isSpecified("Schema")) {
            withAttr(classElement, "schema", classMapping.getSchema());
        }
        if(classMapping.isSpecified("Mutable")) {
            withAttr(classElement, "mutable", String.valueOf(classMapping.getMutable()));
        }
        if(classMapping.isSpecified("Polymorphism")) {
            withAttr(classElement, "polymorphism", classMapping.getPolymorphism());
        }
        if(classMapping.isSpecified("Persister")) {
            withAttr(classElement, "persister", classMapping.getPersister());
        }
        if(classMapping.isSpecified("Where")) {
            withAttr(classElement, "where", classMapping.getWhere());
        }
        if(classMapping.isSpecified("OptimisticLock")) {
            withAttr(classElement, "optimistic-lock", classMapping.getOptimisticLock());
        }
        if(classMapping.isSpecified("Check")) {
            withAttr(classElement, "check", classMapping.getCheck());
        }
        if(classMapping.isSpecified("Name")) {
            withAttr(classElement, "name", classMapping.getName());
        }
        if(classMapping.isSpecified("TableName")) {
            withAttr(classElement, "table", classMapping.getTableName());
        }
        if(classMapping.isSpecified("Proxy")) {
            withAttr(classElement, "proxy", classMapping.getProxy());
        }
        if(classMapping.isSpecified("SelectBeforeUpdate")) {
            withAttr(classElement, "select-before-update", String.valueOf(classMapping.getSelectBeforeUpdate()));
        }
        if(classMapping.isSpecified("Abstract")) {
            withAttr(classElement, "abstract", String.valueOf(classMapping.getAbstract()));
        }
        if(classMapping.isSpecified("Subselect")) {
            withAttr(classElement, "subselect", classMapping.getSubselect());
        }
        if(classMapping.isSpecified("SchemaAction")) {
            withAttr(classElement, "schema-action", classMapping.getSchemaAction());
        }
        if(classMapping.isSpecified("EntityName")) {
            withAttr(classElement, "entity-name", classMapping.getEntityName());
        }
    }

    @Override
    public void visit(IdentityMapping mapping) {
        XmlWriter<IdentityMapping> writer = serviceLocator.getWriter(IdentityMapping.class);
        Document idXml = writer.write(mapping);
        Element idElement = idXml.getRootElement();
        document.getRootElement().add(idElement);
    }
}
