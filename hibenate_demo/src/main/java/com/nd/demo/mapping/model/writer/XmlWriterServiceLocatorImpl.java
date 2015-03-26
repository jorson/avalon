package com.nd.demo.mapping.model.writer;

import com.nd.demo.infrastructure.Container;
import com.nd.demo.infrastructure.ResolveException;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-26
 */
public class XmlWriterServiceLocatorImpl implements XmlWriterServiceLocator {

    private final Container container;

    public XmlWriterServiceLocatorImpl(Container container) {
        this.container = container;
    }

    @Override
    public <T> XmlWriter<T> getWriter() {
        try {
            return (XmlWriter<T>)container.resolve(XmlWriter.class);
        } catch (ResolveException e) {
            return null;
        }
    }
}
