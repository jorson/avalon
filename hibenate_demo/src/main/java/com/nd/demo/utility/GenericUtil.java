package com.nd.demo.utility;

import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.utility
 * @since 2015-03-30
 */
public final class GenericUtil {

    public static final Class getFirstGenericParamClass(Class clazz) {
        return getGenericParamClass(clazz, 0);
    }

    public static final Class getSecondGenericParamClass(Class clazz) {
        return getGenericParamClass(clazz, 1);
    }

    public static final Class getGenericParamClass(Class clazz, int index) {
        List<Class> classList = getGenericParams(clazz);
        if(classList.size() >= (index + 1)) {
            return classList.get(index);
        }
        return null;
    }

    private static List<Class> getGenericParams(Class clazz) {
        if(clazz == null) {
            throw new IllegalArgumentException("class can't be NULL");
        }
        Type superType = clazz.getGenericSuperclass();
        List<Class> resultList = new ArrayList<Class>();

        if(superType instanceof ParameterizedType) {
            Type[] genericTypes = ((ParameterizedType)superType).getActualTypeArguments();
            for(Type type : genericTypes) {
                resultList.add((Class)type);
            }
            return resultList;
        } else {
            throw new IllegalArgumentException("class not a generic class");
        }
    }
}
