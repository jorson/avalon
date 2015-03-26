package com.nd.demo.mapping.model.writer.sorting;

import org.w3c.dom.Node;

import java.util.HashMap;
import java.util.Map;

/**
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer.sorting
 * @since 2015-03-26
 */
public class XmlIdNodeSorter extends BaseXmlNodeSorter {
    @Override
    protected Map<String, SortValue> getSorting() {
        return new HashMap<String, SortValue>() {
            {
                this.put("meta", new SortValue(First, 1));
                this.put("column", new SortValue(Anywhere, 1));
                this.put("generator", new SortValue(Last, 1));
            }
        };
    }

    @Override
    protected void sortChildren(Node node) {
    }
}
