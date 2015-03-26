package com.nd.demo.mapping.model.writer;


import org.w3c.dom.Document;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-23
 */
public interface XmlWriter<T> {

    Document write(T mappingModel);
}
