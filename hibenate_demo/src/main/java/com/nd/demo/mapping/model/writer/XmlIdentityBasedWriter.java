package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.identity.IdMapping;
import com.nd.demo.mapping.model.identity.IdentityMapping;
import com.nd.demo.visitor.NullMappingModelVisitor;
import org.dom4j.Document;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-04-16
 */
public class XmlIdentityBasedWriter extends NullMappingModelVisitor implements XmlWriter<IdentityMapping> {

    private final XmlWriterServiceLocator serviceLocator;
    private Document document;

    public XmlIdentityBasedWriter(XmlWriterServiceLocator serviceLocator) {
        this.serviceLocator = serviceLocator;
    }

    @Override
    public Document write(IdentityMapping mappingModel) {
        document = null;
        mappingModel.acceptVisitor(this);
        return document;
    }

    @Override
    public void processId(IdMapping idMapping) {
        XmlWriter<IdMapping> writer = serviceLocator.getWriter(IdMapping.class);
        document = writer.write(idMapping);
    }
}
