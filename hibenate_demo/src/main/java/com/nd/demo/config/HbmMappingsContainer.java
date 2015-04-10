package com.nd.demo.config;

import com.nd.demo.mapping.ClassMap;
import org.hibernate.cfg.Configuration;

import java.util.ArrayList;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.config
 * @since 2015-04-08
 */
public class HbmMappingsContainer {

    private final List<Class> classes = new ArrayList<Class>();

    private boolean wasUsed;

    HbmMappingsContainer() {

    }

    public HbmMappingsContainer addClass(Class... clazzList) {
        for(Class clz : clazzList) {
            classes.add(clz);
        }
        this.wasUsed = (clazzList.length > 0);
        return this;
    }

    void apply(Configuration cfg) {
        for(Class clz : classes) {
            cfg.addClass(clz);
        }
    }

    boolean isWasUsed() {
        return wasUsed;
    }
}
