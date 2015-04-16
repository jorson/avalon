package com.nd.demo.mapping.model.writer;

import com.nd.demo.infrastructure.Container;
import com.nd.demo.infrastructure.Instantiation;
import com.nd.demo.mapping.Mapping;
import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.PropertyMapping;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.identity.GeneratorMapping;
import com.nd.demo.mapping.model.identity.IdMapping;
import com.nd.demo.mapping.model.identity.IdentityMapping;

import java.util.Objects;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-23
 */
public class XmlWriterContainer extends Container {

    public XmlWriterContainer() {

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<HibernateMapping> writer =
                        new XmlHibernateMappingWriter(new XmlWriterServiceLocatorImpl(getCurrentContainer()));
                return writer;
            }
        }, HibernateMapping.class);

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<ClassMapping> writer =
                        new XmlClassWriter(new XmlWriterServiceLocatorImpl(getCurrentContainer()));
                return writer;
            }
        }, ClassMapping.class);

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<PropertyMapping> writer =
                        new XmlPropertyWriter(new XmlWriterServiceLocatorImpl(getCurrentContainer()));
                return writer;
            }
        }, PropertyMapping.class);

        registerIdWriter();

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<ColumnMapping> writer =
                        new XmlColumnWriter();
                return writer;
            }
        }, ColumnMapping.class);
    }

    private void registerIdWriter() {

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<IdentityMapping> writer =
                        new XmlIdentityBasedWriter(new XmlWriterServiceLocatorImpl(getCurrentContainer()));
                return writer;
            }
        }, IdentityMapping.class);

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<IdMapping> writer =
                        new XmlIdWriter(new XmlWriterServiceLocatorImpl(getCurrentContainer()));
                return writer;
            }
        }, IdMapping.class);

        registerWriter(new Instantiation() {
            @Override
            public Object registeredType(Container container) {
                XmlWriter<GeneratorMapping> writer =
                        new XmlGeneratorWriter();
                return writer;
            }
        }, GeneratorMapping.class);
    }

    private void registerWriter(Instantiation instantiation, Class<? extends Mapping> writerClazz) {
        Object writer = instantiation.registeredType(this);
        register(writer, writerClazz);
    }

    protected XmlWriterContainer getCurrentContainer() {
        return this;
    }
}
