package com.nd.demo.mapping;

import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.Layer;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-04-14
 */
public class ColumnMappingCollection<T> {

    private final List<ColumnMapping> columns = new ArrayList<ColumnMapping>();
    private final T parent;

    public ColumnMappingCollection(T parent) {
        this.parent = parent;
    }

    public T add(String name) {
        ColumnMapping mapping = new ColumnMapping();
        mapping.set(ConstElementKey.ELEMENT_NAME, Layer.USER_SUPPLIED, name);
        columns.add(mapping);
        return parent;
    }

    public T add(String... names) {
        for(String name : names) {
            add(name);
        }
        return parent;
    }

    public T add(ColumnMapping columnMapping) {
        columns.add(columnMapping);
        return parent;
    }

    public T clear()
    {
        columns.clear();
        return parent;
    }

    public int getCount() {
        return this.columns.size();
    }

    public Iterator<ColumnMapping> getIterator() {
        return this.columns.listIterator();
    }
}
