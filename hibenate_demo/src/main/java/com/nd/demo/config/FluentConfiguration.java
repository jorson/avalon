package com.nd.demo.config;

import org.hibernate.SessionFactory;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.service.ServiceRegistry;

import java.util.ArrayList;
import java.util.List;

import static org.hibernate.cfg.Environment.*;

/**
 * @author jorson.WHY
 * @package com.nd.demo.config
 * @since 2015-04-09
 */
public class FluentConfiguration {

    private final String ExceptionMessage = "An invalid or incomplete configuration was used while creating a " +
            "SessionFactory. Check PotentialReasons collection, and InnerException for more detail.";
    private final String ExceptionDatabaseMessage = "Database was not configured through Database method.";
    private final String ExceptionMappingMessage = "No mappings were configured through the Mappings method.";
    private final List<BuildMappingConfiguration> mappingBuilder = new ArrayList<BuildMappingConfiguration>();

    private final Configuration cfg;

    private boolean mappingsSet;

    public FluentConfiguration() {
        this(new Configuration());
    }

    public FluentConfiguration(Configuration cfg) {
        this.cfg = cfg;
    }

    public Configuration buildConfiguration() throws FluentConfigurationException {
        try {
            MappingConfiguration mappingConfiguration = new MappingConfiguration();

            for(BuildMappingConfiguration builder : mappingBuilder) {
                builder.build(mappingConfiguration);
            }
            mappingConfiguration.apply(this.cfg);
            return this.cfg;
        } catch (Exception ex) {
            throw createConfigurationException(ex);
        }
    }

    public FluentConfiguration mappings(BuildMappingConfiguration builder) {
        mappingBuilder.add(builder);
        mappingsSet = true;
        return this;
    }

    public SessionFactory buildSessionFactory() throws FluentConfigurationException {
        try {
            Configuration configuration = buildConfiguration();
            ServiceRegistry serviceRegistry = new StandardServiceRegistryBuilder()
                    .applySettings(configuration.getProperties())
                    .build();
            return configuration.buildSessionFactory(serviceRegistry);
        } catch (Exception ex) {
            throw createConfigurationException(ex);
        }
    }

    private FluentConfigurationException createConfigurationException(Exception innerException) {
        FluentConfigurationException ex = new FluentConfigurationException(ExceptionMessage, innerException);

        if(!mappingsSet) {
            ex.getPotentialReasons().add(ExceptionMappingMessage);
        }

        return ex;
    }

    public interface BuildMappingConfiguration {
        public void build(MappingConfiguration configuration);
    }
}
