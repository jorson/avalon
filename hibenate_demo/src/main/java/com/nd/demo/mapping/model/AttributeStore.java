package com.nd.demo.mapping.model;

import com.nd.demo.mapping.model.collections.AttributeLayeredValues;
import com.nd.demo.mapping.model.collections.LayeredValues;

import java.io.Serializable;

/**
 * Created by Jorson on 2015/3/20.
 */
public class AttributeStore implements Serializable {

    private final AttributeLayeredValues layeredValues;

    public AttributeStore() {
        layeredValues = new AttributeLayeredValues();
    }

    public Object get(String property) {
        LayeredValues values = layeredValues.getLayeredValues(property);
        if(values == null || values.size() == 0) {
            return null;
        }

        Integer maxKey = values.getMaxKey();
        if(maxKey.intValue() == -1) {
            return null;
        }
        return values.get(maxKey);
    }

    public void set(String attribute, Integer layer, Object value) {
        LayeredValues values = layeredValues.getLayeredValues(attribute);
        values.put(layer, value);
    }

    public <T> T getOrDefault(String property) {
        Object result = get(property);
        if(result == null) {
            return null;
        } else {
            return (T)result;
        }
    }

    public void copyTo(AttributeStore theirStore) {
        layeredValues.copyTo(theirStore.layeredValues);
    }

    public AttributeStore clone() {
        AttributeStore clonedStore = new AttributeStore();
        copyTo(clonedStore);
        return clonedStore;
    }

    public boolean isSpecified(String attribute) {
        LayeredValues values = layeredValues.getLayeredValues(attribute);
        return !values.isEmpty();
    }
}
