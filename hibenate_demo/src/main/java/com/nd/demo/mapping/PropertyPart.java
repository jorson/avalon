package com.nd.demo.mapping;

import com.nd.demo.mapping.model.AttributeStore;
import com.nd.demo.mapping.model.Layer;
import com.nd.demo.mapping.model.PropertyMapping;
import com.nd.demo.mapping.model.TypeReference;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.provider.PropertyMappingProvider;

import java.lang.reflect.Field;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-04-14
 */
public class PropertyPart implements PropertyMappingProvider {

    private final Field field;
    private final AttributeStore attributes = new AttributeStore();
    private final Class parentClazz;
    private final ColumnMappingCollection<PropertyPart> columns;

    public PropertyPart(Field field, Class parentClazz) {
        this.field = field;
        this.parentClazz = parentClazz;
        this.columns = new ColumnMappingCollection<PropertyPart>(this);
    }

    @Override
    public PropertyMapping getPropertyMapping() {
        PropertyMapping mapping = new PropertyMapping(attributes.clone());
        mapping.setContainingEntityType(parentClazz);
        mapping.setField(field);

        mapping.set(ConstElementKey.ELEMENT_NAME, Layer.DEFAULTS, mapping.getField().getName());
        mapping.set(ConstElementKey.ELEMENT_TYPE, Layer.DEFAULTS, getDefaultType());

        return mapping;
    }

    public PropertyPart column(String columnName) {
        this.columns.clear();
        this.columns.add(columnName);
        return this;
    }

    private TypeReference getDefaultType() {
        TypeReference reference = new TypeReference(field.getType());
        return reference;
    }
}
