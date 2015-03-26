package com.nd.demo.mapping.model;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.visitor.MappingModelVisitor;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-23
 */
public class ColumnMapping extends MappingBase {

    private final AttributeStore attributes;

    public ColumnMapping() {
        this(new AttributeStore());
    }

    public ColumnMapping(AttributeStore attributes) {
        this.attributes = attributes;
    }

    public ColumnMapping(String defaultColumnName) {
        this();
        set(getName(), Layer.DEFAULTS, defaultColumnName);
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        visitor.processColumn(this);
    }

    @Override
    public boolean isSpecified(String attribute) {
        return false;
    }

    @Override
    public void set(String attribute, int layer, Object value) {

    }

    public String getName() {
        return attributes.getOrDefault("Name");
    }

    public int getLength() {
        return attributes.getOrDefault("Length");
    }

    public boolean getNotNull() {
        return attributes.getOrDefault("NotNull");
    }

    public String getUniqueKey() {
        return attributes.getOrDefault("UniqueKey");
    }

    public String getSqlType() {
        return attributes.getOrDefault("SqlType");
    }

    public String getIndex() {
        return attributes.getOrDefault("Index");
    }

    public String getCheck() {
        return attributes.getOrDefault("Check");
    }

    public int getPrecision() {
        return attributes.getOrDefault("Precision");
    }

    public int getScale() {
        return attributes.getOrDefault("Scale");
    }

    public boolean getUnique() {
        return attributes.getOrDefault("Unique");
    }

    public String getDefault() {
        return attributes.getOrDefault("Default");
    }

    public ColumnMapping clone() {
        return new ColumnMapping(attributes.clone());
    }
}
