package com.nd.demo;

import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.writer.MappingXmlSerializer;
import org.hibernate.cfg.Configuration;
import org.w3c.dom.Document;

import java.util.Iterator;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo
 * @since 2015-03-26
 */
public class PersistenceModel {

    private Iterator<HibernateMapping> compiledMappings;

    public void configure(Configuration cfg) {
        while (compiledMappings.hasNext()) {
            HibernateMapping mapping = compiledMappings.next();
            if(mapping.getClassMappings().size() > 0) {
                MappingXmlSerializer serializer = new MappingXmlSerializer();
                Document document = serializer.serialize(mapping);
                cfg.addDocument(document);
            } else {
                MappingXmlSerializer serializer = new MappingXmlSerializer();
                Document document = serializer.serialize(mapping);

                if(cfg.getClassMapping(mapping.getClassMappings().get(0).getEntityName()) == null) {
                    cfg.addDocument(document);
                }
            }
        }
    }
}
