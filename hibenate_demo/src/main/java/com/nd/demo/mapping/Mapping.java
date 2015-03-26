package com.nd.demo.mapping;

import com.nd.demo.visitor.MappingModelVisitor;

/**
 * Created by Jorson on 2015/3/20.
 */
public interface Mapping {

    void acceptVisitor(MappingModelVisitor visitor);
    boolean isSpecified(String attribute);
    void set(String attribute, int layer, Object value);
}
