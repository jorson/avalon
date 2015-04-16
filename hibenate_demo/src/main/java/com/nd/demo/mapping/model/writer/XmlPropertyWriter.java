package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.PropertyMapping;
import com.nd.demo.visitor.NullMappingModelVisitor;
import org.dom4j.Document;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;

import static com.nd.demo.mapping.model.writer.XmlExtensions.withAttr;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-04-15
 */
public class XmlPropertyWriter extends NullMappingModelVisitor implements XmlWriter<PropertyMapping> {

    private final XmlWriterServiceLocator serviceLocator;
    protected Document document;

    public XmlPropertyWriter(XmlWriterServiceLocator serviceLocator) {
        this.serviceLocator = serviceLocator;
    }

    @Override
    public Document write(PropertyMapping mappingModel) {
        document = null;
        mappingModel.acceptVisitor(this);
        return document;
    }

    @Override
    public void processProperty(PropertyMapping propertyMapping) {
        document = DocumentHelper.createDocument();
        Element propertyElement = document.addElement("property");

        if(propertyMapping.isSpecified("Access")) {
            withAttr(propertyElement, "access", propertyMapping.getAccess());
        }
        if(propertyMapping.isSpecified("Generated")) {
            withAttr(propertyElement, "generated", propertyMapping.getGenerated());
        }
        if(propertyMapping.isSpecified("Name")) {
            withAttr(propertyElement,"name", propertyMapping.getName());
        }
        if(propertyMapping.isSpecified("OptimisticLock")) {
            withAttr(propertyElement,"optimistic-lock", String.valueOf(propertyMapping.getOptimisticLock()));
        }
        if(propertyMapping.isSpecified("Insert")) {
            withAttr(propertyElement,"insert", String.valueOf(propertyMapping.getInsert()));
        }
        if(propertyMapping.isSpecified("Update")) {
            withAttr(propertyElement,"update", String.valueOf(propertyMapping.getUpdate()));
        }
        if(propertyMapping.isSpecified("Formula")) {
            withAttr(propertyElement,"formula", propertyMapping.getFormula());
        }
        if(propertyMapping.isSpecified("Type")) {
            withAttr(propertyElement,"type", propertyMapping.getType().toString());
        }
        if(propertyMapping.isSpecified("Lazy")) {
            withAttr(propertyElement,"lazy", String.valueOf(propertyMapping.getLazy()));
        }
    }

    @Override
    public void visit(ColumnMapping columnMapping) {
        XmlWriter<ColumnMapping> writer  = serviceLocator.getWriter(ColumnMapping.class);
        Document columnXml = writer.write(columnMapping);
        XmlExtensions.importAndAppendChild(document, columnXml);
    }
}
