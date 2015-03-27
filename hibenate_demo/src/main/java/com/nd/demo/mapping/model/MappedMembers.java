package com.nd.demo.mapping.model;

import com.nd.demo.mapping.Mapping;
import com.nd.demo.visitor.MappingModelVisitor;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

/**
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-27
 */
public class MappedMembers implements Mapping, HasMappedMembers {

    private final Collection<PropertyMapping> properties;

    public MappedMembers() {
        properties = new ArrayList<PropertyMapping>();
    }

    @Override
    public Collection<PropertyMapping> getProperties() {
        return properties;
    }

    @Override
    public void addProperty(PropertyMapping property) {
        for(PropertyMapping mapping : properties) {
            if(mapping.getName().equals(property.getName())) {
                throw new UnsupportedOperationException("Tried to add property '"
                        + property.getName() + "' when already added.");
            }
        }
        properties.add(property);
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        for(PropertyMapping mapping : properties) {
            visitor.visit(mapping);
        }
    }

    @Override
    public boolean isSpecified(String attribute) {
        return false;
    }

    @Override
    public void set(String attribute, int layer, Object value) {

    }
}
