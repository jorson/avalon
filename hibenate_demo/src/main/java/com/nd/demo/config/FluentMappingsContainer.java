package com.nd.demo.config;

import com.nd.demo.PersistenceModel;

import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.config
 * @since 2015-04-09
 */
public class FluentMappingsContainer {

    private final List<Class> classes = new ArrayList<Class>();
    private boolean wasUsed = false;

    public FluentMappingsContainer addClass(Class... clazzList) {
        if(clazzList == null || clazzList.length == 0) {
            throw new IllegalArgumentException("clazzList");
        }

        for(Class clazz : clazzList) {
            this.classes.add(clazz);
        }
        this.wasUsed = true;
        return this;
    }

    void apply(PersistenceModel model) {
        for(Class clazz : classes) {
            model.add(clazz);
        }
    }

    public boolean isWasUsed() {
        return wasUsed;
    }

    public void setWasUsed(boolean wasUsed) {
        this.wasUsed = wasUsed;
    }
}
