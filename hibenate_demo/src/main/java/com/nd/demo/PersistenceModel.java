package com.nd.demo;

import com.nd.demo.mapping.model.HibernateMapping;
import org.hibernate.cfg.Configuration;

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

            } else {
                if(cfg.getClassMapping(mapping.getClassMappings().get(0).))
            }
        }
    }
}
