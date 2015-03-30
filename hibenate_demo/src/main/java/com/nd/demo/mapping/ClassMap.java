package com.nd.demo.mapping;


import com.nd.demo.mapping.model.AttributeStore;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.Layer;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.provider.HibernateMappingProvider;
import com.nd.demo.mapping.provider.MappingProvider;
import com.nd.demo.mapping.provider.PropertyMappingProvider;
import com.nd.demo.utility.GenericUtil;
import org.apache.commons.lang3.StringUtils;

import java.lang.reflect.Field;

public class ClassMap<T> extends ClassLikeMapBase<T> implements MappingProvider {

    protected final AttributeStore attributes;
    private final MappingProviderStore providers;
    private final HibernateMappingPart hibernateMappingPart = new HibernateMappingPart();

    public ClassMap(AttributeStore attributes, MappingProviderStore providers) {
        super(providers);
        this.attributes = attributes;
        this.providers = providers;
    }

    public void table(String tableName) {
        attributes.set("TableName", Layer.USER_SUPPLIED, tableName);
    }

    public IdentityPart id(Field member) {
        return id(member, null);
    }

    public IdentityPart id(Field member, String columnName) {
        IdentityPart part = new IdentityPart(getClazz(), member);
        if(!StringUtils.isEmpty(columnName)) {
            part.column(columnName);
        }
        providers.setId(part);
        return part;
    }

    @Override
    public ClassMapping getClassMapping() {
        ClassMapping mapping = new ClassMapping(attributes.clone());

        mapping.set(mapping.getClazz().getName(), Layer.DEFAULTS,
                GenericUtil.getFirstGenericParamClass(getClass()));
        mapping.set(mapping.getName(), Layer.DEFAULTS,
                GenericUtil.getFirstGenericParamClass(getClass()).getSimpleName());

        for(PropertyMappingProvider property : providers.getProperties()) {
            mapping.addProperty(property.getPropertyMapping());
        }

        if(providers.getId() != null) {
            mapping.set(mapping.getId().toString(), Layer.DEFAULTS,
                    providers.getId().getIdentityMapping());
        }

        mapping.set(mapping.getTableName(), Layer.DEFAULTS,
                getDefaultTableName());

        return mapping;
    }

    @Override
    public HibernateMapping getHibernateMapping() {
        HibernateMapping mapping = ((HibernateMappingProvider)hibernateMappingPart).getHibernateMapping();
        return mapping;
    }

    private String getDefaultTableName() {
        String tableName = getClazz().getSimpleName();
        return "'" + tableName + "'";
    }
}
