package com.nd.demo.mapping.model.writer;

import com.nd.demo.infrastructure.Container;
import com.nd.demo.infrastructure.ResolveException;
import com.nd.demo.mapping.model.HibernateMapping;

/**
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-23
 */
public final class XmlWriterFactory {

    private static final Container container = new XmlWriterContainer();

    public static XmlWriter<HibernateMapping> createHibernateMappingWriter() throws ResolveException {
        Object writer = container.resolve(HibernateMapping.class);
        return (XmlWriter<HibernateMapping>)writer;
    }
}
