package com.nd.demo.mapping;

import com.nd.demo.mapping.model.AttributeStore;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.Layer;
import com.nd.demo.mapping.provider.HibernateMappingProvider;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-30
 */
public class HibernateMappingPart implements HibernateMappingProvider {

    private final AttributeStore attributes = new AttributeStore();
    private boolean nextBool = true;

    public HibernateMappingPart schema(String schema) {
        attributes.set("Schema", Layer.USER_SUPPLIED, schema);
        return this;
    }

    public HibernateMappingPart autoImpor() {
        attributes.set("AutoImport", Layer.USER_SUPPLIED, nextBool);
        nextBool = true;
        return this;
    }

    public HibernateMappingPart defaultLazy() {
        attributes.set("DefaultLazy", Layer.USER_SUPPLIED, nextBool);
        nextBool = true;
        return this;
    }

    public HibernateMappingPart catalog(String catalog) {
        attributes.set("Catalog", Layer.USER_SUPPLIED, catalog);
        return this;
    }

    public HibernateMappingPart basePackage(String basePkg) {
        attributes.set("Package", Layer.USER_SUPPLIED, basePkg);
        return this;
    }

    @Override
    public HibernateMapping getHibernateMapping() {
        return new HibernateMapping(attributes.clone());
    }
}
