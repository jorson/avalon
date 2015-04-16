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
public class XmlClassLikeNodeSorter extends BaseXmlNodeSorter {
    @Override
    protected Map<String, SortValue> getSorting() {
        return new HashMap<String, SortValue>()
        {{
                //Top Section
                this.put("meta", new SortValue(First, 1));
                this.put("subselect", new SortValue(First, 2));
                this.put("subselect", new SortValue(First, 2));
                this.put("cache", new SortValue(First, 3));
                this.put("synchronize", new SortValue(First, 4));
                this.put("comment", new SortValue(First, 5));
                this.put("tuplizer", new SortValue(First, 6));
                this.put("key", new SortValue(First, 7));
                this.put("parent", new SortValue(First, 7));
                this.put("id", new SortValue(First, 7));
                this.put("composite-id", new SortValue(First, 7));
                this.put("discriminator", new SortValue(First, 8));
                this.put("natural-id", new SortValue(First, 9));
                this.put("version", new SortValue(First, 10));
                this.put("timestamp", new SortValue(First, 10));
                //Middle Section
                this.put("property", new SortValue(Anywhere,1));
                this.put("many-to-one", new SortValue(Anywhere,1));
                this.put("one-to-one", new SortValue(Anywhere, 1));
                this.put("component", new SortValue(Anywhere, 1));
                this.put("dynamic-component", new SortValue(Anywhere, 1));
                this.put("properties", new SortValue(Anywhere, 1));
                this.put("any", new SortValue(Anywhere,1));
                this.put("map", new SortValue(Anywhere,1));
                this.put("set", new SortValue(Anywhere,1));
                this.put("list", new SortValue(Anywhere,1));
                this.put("bag", new SortValue(Anywhere,1));
                this.put("idbag", new SortValue(Anywhere,1));
                this.put("array", new SortValue(Anywhere,1));
                this.put("primitive-array", new SortValue(Anywhere,1));

                //Bottom Section
                this.put("join", new SortValue(Last,1));
                this.put("subclass", new SortValue(Last,2));
                this.put("joined-subclass", new SortValue(Last,3));
                this.put("union-subclass", new SortValue(Last,4));
                this.put("loader", new SortValue(Last,5));
                this.put("sql-insert", new SortValue(Last,6));
                this.put("sql-update", new SortValue(Last,7));
                this.put("sql-delete", new SortValue(Last,8));
                this.put("filter", new SortValue(Last,9));
                this.put("query", new SortValue(Last,10));
                this.put("sql-query", new SortValue(Last,11));
        }};
    }

    @Override
    protected void sortChildren(Node node) {
        if(node.getNodeName() == "subclass" || node.getNodeName() == "joined-subclass"
                 || node.getNodeName() == "union-subclass" || node.getNodeName() == "component") {

        } else if(node.getNodeName() == "id") {
            new XmlIdNodeSorter().sort(node);
        } else if(isCollection(node.getNodeName())) {
            new XmlCollectionNodeSorter().sort(node);
        }
    }

    private boolean isCollection(String name) {
        return name == "bag" || name == "set" || name == "list" || name == "map" ||
                name == "array" || name == "primitive-array";
    }
}
