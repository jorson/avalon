package com.nd.demo.mapping.model.writer;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-25
 */
public interface XmlWriterServiceLocator {

    <T> XmlWriter<T> getWriter(Class clazz);
}
