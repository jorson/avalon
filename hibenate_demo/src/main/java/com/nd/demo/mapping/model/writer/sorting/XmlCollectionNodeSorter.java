package com.nd.demo.mapping.model.writer.sorting;

import org.w3c.dom.Node;

import java.util.HashMap;
import java.util.Map;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer.sorting
 * @since 2015-03-26
 */
public class XmlCollectionNodeSorter extends BaseXmlNodeSorter {
    @Override
    protected Map<String, SortValue> getSorting() {
        return new HashMap<String, SortValue>() {
            {
                this.put("meta", new SortValue(First, 1));
                this.put("jcs-cache", new SortValue(First, 1));
                this.put("cache", new SortValue(First, 1));
                this.put("key", new SortValue(First, 3));
                this.put("index", new SortValue(First, 4));
                this.put("list-index", new SortValue(First, 4));
                this.put("index-many-to-many", new SortValue(First, 4));
                this.put("element", new SortValue(Anywhere, 1));
                this.put("one-to-many", new SortValue(Anywhere, 1));
                this.put("many-to-many", new SortValue(Anywhere, 1));
                this.put("composite-element", new SortValue(Anywhere, 1));
                this.put("many-to-any", new SortValue(Anywhere, 1));
                this.put("filter", new SortValue(Last, 1));

            }
        };
    }

    @Override
    protected void sortChildren(Node node) {

    }
}
