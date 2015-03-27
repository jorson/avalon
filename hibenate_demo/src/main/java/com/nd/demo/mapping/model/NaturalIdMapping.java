package com.nd.demo.mapping.model;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.visitor.MappingModelVisitor;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-27
 */
public class NaturalIdMapping extends MappingBase {

    private final AttributeStore attributes;
    private final List<PropertyMapping> properties = new ArrayList<PropertyMapping>();

    public NaturalIdMapping() {
        this(new AttributeStore());
    }

    public NaturalIdMapping(AttributeStore attributes) {
        this.attributes = attributes;
    }

    public boolean getMutable() {
        return attributes.getOrDefault("Mutable");
    }

    public Collection<PropertyMapping> getProperties() {
        return properties;
    }

    public void addProperty(PropertyMapping mapping) {
        properties.add(mapping);
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        visitor.processNaturalId(this);

        for(PropertyMapping key : properties) {
            visitor.visit(key);
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
}
