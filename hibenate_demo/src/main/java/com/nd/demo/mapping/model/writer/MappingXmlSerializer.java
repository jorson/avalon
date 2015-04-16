package com.nd.demo.mapping.model.writer;

import com.nd.demo.infrastructure.ResolveException;
import com.nd.demo.mapping.model.HibernateMapping;
import org.dom4j.Document;

/**
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-23
 */
public class MappingXmlSerializer {

    public Document serialize(HibernateMapping mapping) {
        return buildXml(mapping);
    }

    private static Document buildXml(HibernateMapping rootMapping) {
        try {
            XmlWriter writer = XmlWriterFactory.createHibernateMappingWriter();
            return writer.write(rootMapping);
        } catch (ResolveException e) {
            e.printStackTrace();
        }
        return null;
    }
}
