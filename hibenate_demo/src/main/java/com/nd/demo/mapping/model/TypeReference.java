package com.nd.demo.mapping.model;

import java.lang.reflect.Type;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-25
 */
public class TypeReference {

    public static final TypeReference empty = new TypeReference("nop");

    private Class innerClazz;
    private String innerName;

    public TypeReference(String name) {
        try {
            this.innerClazz = Class.forName(name, false, null);
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }
        this.innerName = name;
    }

    public TypeReference(Class clazz) {
        innerClazz = clazz;
        innerName = clazz.getName();
    }

    public String getInnerName() {
        return innerName;
    }
}
