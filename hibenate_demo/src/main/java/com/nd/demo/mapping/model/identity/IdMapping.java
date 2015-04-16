package com.nd.demo.mapping.model.identity;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.mapping.model.AttributeStore;
import com.nd.demo.mapping.model.ColumnBasedMappingBase;
import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.TypeReference;
import com.nd.demo.visitor.MappingModelVisitor;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-23
 */
public class IdMapping extends ColumnBasedMappingBase implements IdentityMapping {

    private Class containingEntityType;

    public IdMapping() {
        this(new AttributeStore());
    }

    public IdMapping(AttributeStore underlyingStore) {
        super(underlyingStore);
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        visitor.processId(this);

        for(ColumnMapping mapping : getColumns()) {
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

    public String getAccess() {
        return null;
    }

    public String getName() {
        return null;
    }

    public TypeReference getType() {
        return null;
    }

    public String getUnsavedValue() {
        return null;
    }

    public Class getContainingEntityType() {
        return containingEntityType;
    }

    public void setContainingEntityType(Class containingEntityType) {
        this.containingEntityType = containingEntityType;
    }
}
