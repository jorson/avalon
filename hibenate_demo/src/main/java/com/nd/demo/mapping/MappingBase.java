package com.nd.demo.mapping;

import com.nd.demo.visitor.MappingModelVisitor;

/**
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-23
 */
public abstract class MappingBase implements Mapping {

    public abstract void acceptVisitor(MappingModelVisitor visitor);
    public abstract boolean isSpecified(String attribute);
    public abstract void set(String attribute, int layer, Object value);
}
