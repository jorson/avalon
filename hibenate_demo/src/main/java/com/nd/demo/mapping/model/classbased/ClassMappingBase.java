package com.nd.demo.mapping.model.classbased;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.mapping.model.*;
import com.nd.demo.visitor.MappingModelVisitor;

import java.util.Collection;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.classbased
 * @since 2015-03-26
 */
public abstract class ClassMappingBase extends MappingBase implements HasMappedMembers {

    private final AttributeStore attributes;
    private final MappedMembers mappedMembers;

    protected ClassMappingBase(AttributeStore attributes) {
        this.attributes = attributes;
        mappedMembers = new MappedMembers();
    }

    public abstract String getName();
    public abstract Class getClazz();

    @Override
    public Collection<PropertyMapping> getProperties() {
        return mappedMembers.getProperties();
    }

    @Override
    public void addProperty(PropertyMapping property) {
        mappedMembers.addProperty(property);
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        mappedMembers.acceptVisitor(visitor);
    }
}
