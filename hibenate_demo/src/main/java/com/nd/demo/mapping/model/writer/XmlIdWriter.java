package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.identity.GeneratorMapping;
import com.nd.demo.mapping.model.identity.IdMapping;
import com.nd.demo.visitor.NullMappingModelVisitor;
import org.dom4j.Document;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;

import static com.nd.demo.mapping.model.writer.XmlExtensions.*;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-23
 */
public class XmlIdWriter extends NullMappingModelVisitor implements XmlWriter<IdMapping> {

    private Document document;
    private final XmlWriterServiceLocator serviceLocator;

    public XmlIdWriter(XmlWriterServiceLocator locator) {
        this.serviceLocator = locator;
    }

    @Override
    public Document write(IdMapping mappingModel) {
        this.document = null;
        mappingModel.acceptVisitor(this);
        return document;
    }

    @Override
    public void processId(IdMapping mapping) {
        document = DocumentHelper.createDocument();
        Element element = document.addElement("id");

        if(mapping.isSpecified("Access")) {
            withAttr(element, "access", mapping.getAccess());
        }
        if(mapping.isSpecified("Name")) {
            withAttr(element, "name", mapping.getName());
        }
        if(mapping.isSpecified("Type")) {
            withAttr(element, "type", mapping.getType());
        }
        if(mapping.isSpecified("UnsavedValue")) {
            withAttr(element, "unsaved-value", mapping.getUnsavedValue());
        }
    }

    @Override
    public void visit(ColumnMapping columnMapping) {
        XmlWriter<ColumnMapping> writer = this.serviceLocator.getWriter(ColumnMapping.class);
        Document columnXml = writer.write(columnMapping);
        importAndAppendChild(this.document, columnXml);
    }

    @Override
    public void visit(GeneratorMapping generatorMapping) {
        XmlWriter<GeneratorMapping> writer = this.serviceLocator.getWriter(GeneratorMapping.class);
        Document generatorXml = writer.write(generatorMapping);
        importAndAppendChild(this.document, generatorXml);
    }
}
