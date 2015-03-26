package com.nd.demo.infrastructure;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.infrastructure
 * @since 2015-03-23
 */
public class ResolveException extends Exception {

    public ResolveException(Class clazz) {
        super("Unable to resolve dependency:'" + clazz.getName() + "'");
    }
}
