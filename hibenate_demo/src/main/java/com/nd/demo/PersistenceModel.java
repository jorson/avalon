package com.nd.demo;

import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.writer.MappingXmlSerializer;
import com.nd.demo.mapping.provider.MappingProvider;
import com.nd.demo.visitor.MappingModelVisitor;
import org.hibernate.cfg.Configuration;
import org.w3c.dom.Document;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo
 * @since 2015-03-26
 */
public class PersistenceModel {

    protected final List<MappingProvider> classProviders = new ArrayList<MappingProvider>();
    private final List<MappingModelVisitor> visitors = new ArrayList<MappingModelVisitor>();
    private List<HibernateMapping> compiledMappings;

    public PersistenceModel() {

    }

    public void configure(Configuration cfg) {

        ensureMappingsBuilt();

        for(HibernateMapping mapping : compiledMappings) {
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

    private void ensureMappingsBuilt() {

    }

    private void applyVisitors(List<HibernateMapping> mappings) {

    }
}
