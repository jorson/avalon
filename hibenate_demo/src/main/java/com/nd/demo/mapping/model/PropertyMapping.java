package com.nd.demo.mapping.model;

import com.nd.demo.visitor.MappingModelVisitor;

import java.io.File;
import java.lang.reflect.Field;
import java.util.Collection;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-26
 */
public class PropertyMapping extends ColumnBasedMappingBase {

    private Class containingEntityType;
    private Field field;

    public PropertyMapping() {
        this(new AttributeStore());
    }

    public PropertyMapping(AttributeStore underlyingStore) {
        super(underlyingStore);
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        visitor.processProperty(this);

        Collection<ColumnMapping> columnMappings = getColumns();
        for(ColumnMapping column : columnMappings) {
            visitor.visit(column);
        }
    }

    @Override
    public boolean isSpecified(String attribute) {
        return attributes.isSpecified(attribute);
    }

    @Override
    public void set(String attribute, int layer, Object value) {
        attributes.set(attribute, layer, value);
    }

    public String getName() {
        return attributes.getOrDefault("Name");
    }

    public String getAccess() {
        return attributes.getOrDefault("Access");
    }

    public boolean getInsert() {
        return attributes.getOrDefault("Insert");
    }

    public boolean getUpdate() {
        return attributes.getOrDefault("Update");
    }

    public String getFormula() {
        return attributes.getOrDefault("Formula");
    }

    public boolean getLazy() {
        return attributes.getOrDefault("Lazy");
    }

    public boolean getOptimisticLock() {
        return attributes.getOrDefault("OptimisticLock");
    }

    public String getGenerated() {
        return attributes.getOrDefault("Generated");
    }

    public TypeReference getType() {
        return attributes.getOrDefault("Type");
    }

    public Class getContainingEntityType() {
        return containingEntityType;
    }

    public void setContainingEntityType(Class containingEntityType) {
        this.containingEntityType = containingEntityType;
    }

    public Field getField() {
        return field;
    }

    public void setField(Field field) {
        this.field = field;
    }
}
