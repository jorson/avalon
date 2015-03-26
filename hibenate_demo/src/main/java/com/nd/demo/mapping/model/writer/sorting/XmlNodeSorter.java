package com.nd.demo.mapping.model.writer.sorting;

import org.w3c.dom.Node;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer.sorting
 * @since 2015-03-26
 */
public class XmlNodeSorter {

    public static Node sortClassChildren(Node node) {
        return new XmlClasslikeNodeSorter().sort(node);
    }
}
