package com.nd.demo.mapping.model;

import java.io.Serializable;

public class ClassMapping implements Serializable {

    public final AttributeStore attributeStore;

    public ClassMapping() {
        this(new AttributeStore());
    }

    public ClassMapping(AttributeStore attributes) {
        this.attributeStore = attributes;
    }

    public Id
}
