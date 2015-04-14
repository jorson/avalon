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
import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.util.HashMap;
import java.util.Map;

public class ClassMap<T> extends ClassLikeMapBase<T> implements MappingProvider {

    protected final AttributeStore attributes;
    private final MappingProviderStore providers;
    private final HibernateMappingPart hibernateMappingPart = new HibernateMappingPart();

    protected final Map<String, Field> entryFields;

    public ClassMap() {
        this(new AttributeStore(), new MappingProviderStore());
    }

    public ClassMap(AttributeStore attributes, MappingProviderStore providers) {
        super(providers);
        this.attributes = attributes;
        this.providers = providers;
        this.entryFields = new HashMap<String, Field>();
        getEntryFields();
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

        mapping.set(ConstElementKey.ELEMENT_TYPE, Layer.DEFAULTS,
                GenericUtil.getFirstGenericParamClass(getClass()));
        mapping.set(ConstElementKey.ELEMENT_NAME, Layer.DEFAULTS,
                GenericUtil.getFirstGenericParamClass(getClass()).getSimpleName());

        for(PropertyMappingProvider property : providers.getProperties()) {
            mapping.addProperty(property.getPropertyMapping());
        }

        if(providers.getId() != null) {
            mapping.set(ConstElementKey.ELEMENT_ID, Layer.DEFAULTS, providers.getId().getIdentityMapping());
        }
        mapping.set(ConstElementKey.ELEMENT_TABLE_NAME, Layer.DEFAULTS, getDefaultTableName());

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

    private void getEntryFields() {
        Type superClazz = getClass().getGenericSuperclass();
        Class paramClazz = (Class)((ParameterizedType)superClazz).getActualTypeArguments()[0];

        Field[] fields = paramClazz.getDeclaredFields();
        for(Field field : fields) {
            this.entryFields.put(field.getName().toLowerCase(), field);
        }
    }

    public Field getField(String name) {
        Field field = this.entryFields.get(name.toLowerCase());
        if(field == null) {
            throw new IllegalArgumentException("can not found field!");
        }
        return field;
    }
}
