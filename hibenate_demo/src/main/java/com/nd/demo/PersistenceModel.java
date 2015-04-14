package com.nd.demo;

import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.writer.MappingXmlSerializer;
import com.nd.demo.mapping.provider.MappingProvider;
import com.nd.demo.visitor.MappingModelVisitor;
import com.nd.demo.visitor.SeparateSubclassVisitor;
import com.nd.demo.visitor.ValidationVisitor;
import org.hibernate.cfg.Configuration;
import org.w3c.dom.Document;

import java.lang.reflect.Constructor;
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
    private ValidationVisitor validationVisitor;

    public PersistenceModel() {
/*        visitors.add(new SeparateSubclassVisitor());
        visitors.add(validationVisitor = new ValidationVisitor());*/
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

    public void add(Class clazz) {
        try {
            Object mapping = instantiateUsingParameterlessConstructor(clazz);

            if(mapping instanceof MappingProvider) {
                add((MappingProvider)mapping);
            } else {
                throw new UnsupportedOperationException("unsupported mapping type '" +
                clazz.getName() + "'");
            }

        } catch (MissingConstructorException e) {
            e.printStackTrace();
        }
    }

    public void add(MappingProvider provider) {
        classProviders.add(provider);
    }

    public List<HibernateMapping> buildMappings() {
        final List<HibernateMapping> hibernateMappings = new ArrayList<HibernateMapping>();
        buildSeparateMappings(new AddHibernateMapping() {
            @Override
            public void addMapping(HibernateMapping hibernateMapping) {
                hibernateMappings.add(hibernateMapping);
            }
        });

        applyVisitors(hibernateMappings);

        return hibernateMappings;
    }

    private void buildSeparateMappings(AddHibernateMapping addHibernateMapping) {
        for(MappingProvider classMap : classProviders) {
            HibernateMapping hbm = classMap.getHibernateMapping();
            hbm.addClass(classMap.getClassMapping());
            addHibernateMapping.addMapping(hbm);
        }
    }

    private void ensureMappingsBuilt() {
        if(compiledMappings != null) {
            return;
        }
        compiledMappings = buildMappings();
    }

    private void applyVisitors(List<HibernateMapping> mappings) {
        for(MappingModelVisitor visitor : visitors) {
            visitor.visit(mappings);
        }
    }

    private Object instantiateUsingParameterlessConstructor(Class clazz) throws MissingConstructorException {
        try {
            Constructor constructor = clazz.getConstructor(null);
            return constructor.newInstance(null);
        } catch (Exception ex) {
            throw new MissingConstructorException(clazz);
        }
    }

    public interface AddHibernateMapping {
        public void addMapping(HibernateMapping hibernateMapping);
    }
}
