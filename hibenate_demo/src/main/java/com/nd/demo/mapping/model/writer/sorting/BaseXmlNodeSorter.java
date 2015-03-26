package com.nd.demo.mapping.model.writer.sorting;

import org.w3c.dom.Node;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer.sorting
 * @since 2015-03-26
 */
public abstract class BaseXmlNodeSorter {

    //TODO: no complete

    protected final int First = 0;    // Top
    protected final int Anywhere = 1; // Middle
    protected final int Last = 2;     // Bottom

    public Node sort(Node node)
    {
        List<Node> children = new ArrayList<Node>();
        Map<String, SortValue> sorting = getSorting();

        int childNodeCount = node.getChildNodes().getLength();
        for(int i=0; i<childNodeCount; i++) {
            Node childNode = node.getChildNodes().item(i);
            childNode.appendChild(childNode);
            sortChildren(childNode);
        }

        while (node.getChildNodes().getLength() > 0) {
            node.removeChild(node.getFirstChild());
        }

        for(Node child : children) {
            node.appendChild(child);
        }
        return node;
    }

    protected abstract Map<String, SortValue> getSorting();
    protected abstract void sortChildren(Node node);
}
