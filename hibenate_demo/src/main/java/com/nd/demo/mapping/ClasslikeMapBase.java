package com.nd.demo.mapping;

import com.nd.demo.utility.GenericUtil;

import java.lang.reflect.Field;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-30
 */
public abstract class ClassLikeMapBase<T> {

    private final MappingProviderStore providers;

    protected ClassLikeMapBase(MappingProviderStore providers) {
        this.providers = providers;
    }


    public void map(Field member) {

    }

    public void map(Field member, String columnName) {

    }

    protected Class getClazz() {
        return GenericUtil.getFirstGenericParamClass(getClass());
    }
}
