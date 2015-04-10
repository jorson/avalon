package com.nd.demo;

import com.nd.demo.config.FluentConfiguration;
import com.nd.demo.config.FluentConfigurationException;
import com.nd.demo.config.Fluently;
import com.nd.demo.config.MappingConfiguration;
import com.nd.demo.entry.User;
import com.nd.demo.map.UserMap;
import org.hibernate.SessionFactory;
import org.junit.Assert;
import org.junit.Test;

/**
 * @author jorson.WHY
 * @package com.nd.demo
 * @since 2015-04-09
 */
public class FluentTest {

    @Test
    public void createSessionFactory() throws FluentConfigurationException {

        SessionFactory factory = Fluently.configure().mappings(new FluentConfiguration.BuildMappingConfiguration() {
            @Override
            public void build(MappingConfiguration configuration) {
                configuration.getFluentMappingsContainer().addClass(new Class[] {UserMap.class});
            }
        }).buildSessionFactory();

        Assert.assertNotNull(factory);
    }
}
