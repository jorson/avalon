package com.nd.demo.mapping.model.writer;

import com.nd.demo.infrastructure.Container;
import com.nd.demo.infrastructure.Instantiation;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-23
 */
public class XmlWriterContainer extends Container {

    public XmlWriterContainer() {
        register(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                return new XmlWriterServiceLocatorImpl(getCurrentContainer());
            }
        }, XmlWriterContainer.class);

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                return null;
            }
        }, XmlHibernateMappingWriter.class);
    }

    private void registerWriter(Instantiation instantiation, Class<? extends XmlWriter> writerClazz) {
        register(instantiation, writerClazz);
    }

    protected XmlWriterContainer getCurrentContainer() {
        return this;
    }
}
