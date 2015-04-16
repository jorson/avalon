package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.TypeReference;
import org.dom4j.Attribute;
import org.dom4j.Document;
import org.dom4j.Element;
import org.dom4j.dom.DOMAttribute;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-25
 */
final class XmlExtensions {

    public static final Element withAttr(Element element, String key, boolean value) {
        return withAttr(element, key, String.valueOf(value).toLowerCase());
    }

    public static final Element withAttr(Element element, String key, int value) {
        return withAttr(element, key, String.valueOf(value));
    }

    public static final Element withAttr(Element element, String key, String attrValue) {
        element.addAttribute(key, attrValue);
        return element;
    }

    public static final Element withAttr(Element element, String key, TypeReference value) {
        return withAttr(element, key, value.toString());
    }

    public static final void importAndAppendChild(Document document, Document toImport) {
        Element rootElement = toImport.getRootElement();
        document.getRootElement().add(rootElement);
    }
}
