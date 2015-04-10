package com.nd.demo;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo
 * @since 2015-04-09
 */
public class MissingConstructorException extends Exception {

    public MissingConstructorException(Class clazz) {
        super("'" + clazz.getSimpleName() + "' is missing a parameterless constructor");
    }
}
