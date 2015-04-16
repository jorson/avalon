package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.PropertyMapping;
import com.nd.demo.visitor.NullMappingModelVisitor;
import org.dom4j.Document;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-04-14
 */
public class XmlClassWriterBase extends NullMappingModelVisitor {

    private final XmlWriterServiceLocator serviceLocator;
    protected Document document;

    protected XmlClassWriterBase(XmlWriterServiceLocator serviceLocator) {
        this.serviceLocator = serviceLocator;
    }

    @Override
    public void visit(PropertyMapping propertyMapping) {
        XmlWriter<PropertyMapping> writer = serviceLocator.getWriter(PropertyMapping.class);
        Document doc = writer.write(propertyMapping);
        XmlExtensions.importAndAppendChild(this.document, doc);
    }
}
