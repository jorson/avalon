package com.nd.demo.mapping;

import com.nd.demo.mapping.model.AttributeStore;
import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.Layer;
import com.nd.demo.mapping.model.TypeReference;
import com.nd.demo.mapping.model.identity.GeneratorMapping;
import com.nd.demo.mapping.model.identity.IdMapping;
import com.nd.demo.mapping.provider.IdentityMappingProvider;
import org.apache.commons.lang3.StringUtils;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-30
 */
public class IdentityPart implements IdentityMappingProvider {

    private final AttributeStore columnAttributes = new AttributeStore();
    private final AttributeStore attributes = new AttributeStore();
    private final List<String> columns = new ArrayList<String>();

    private final Class entityClazz;
    private Class identityType;
    private boolean nextBool = true;
    private String name;
    private Field field;


    public IdentityPart(Class entity, Field field) {
        this.entityClazz = entity;
        this.field = field;
        this.identityType = field.getType();

        setName(field.getName());
        setDefaultGenerator();
    }

    @Override
    public IdMapping getIdentityMapping() {
        IdMapping mapping = new IdMapping(attributes.clone());
        mapping.setContainingEntityType(entityClazz);

        if(columns.size() > 0) {
            for(String column : columns) {
                ColumnMapping columnMapping = new ColumnMapping(columnAttributes.clone());
                columnMapping.set(ConstElementKey.ELEMENT_NAME, Layer.DEFAULTS, column);
                mapping.addColumn(Layer.USER_SUPPLIED, columnMapping);
            }
        } else if(hasNameSpecified()) {
            ColumnMapping columnMapping = new ColumnMapping(columnAttributes.clone());
            columnMapping.set(ConstElementKey.ELEMENT_NAME, Layer.DEFAULTS, name);
            mapping.addColumn(Layer.DEFAULTS, columnMapping);
        }

        if(this.field != null) {
            mapping.set(ConstElementKey.ELEMENT_NAME, Layer.DEFAULTS, name);
        }
        mapping.set(ConstElementKey.ELEMENT_TYPE, Layer.DEFAULTS, new TypeReference(identityType));

        return mapping;
    }

    private void setDefaultGenerator() {
        GeneratorMapping generatorMapping = new GeneratorMapping();
/*        GeneratorBuilder defaultGenerator = new GeneratorBuilder(generatorMapping,
                )*/
    }

    public IdentityPart column(String columnName) {
        columns.clear();
        columns.add(columnName);
        return this;
    }

    void setName(String newName) {
        this.name = newName;
    }

    boolean hasNameSpecified() {
        return !StringUtils.isEmpty(this.name);
    }
}
