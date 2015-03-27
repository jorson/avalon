package com.nd.demo.mapping.model;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.mapping.model.collections.LayeredColumns;

import java.util.Collection;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-26
 */
public abstract class ColumnBasedMappingBase extends MappingBase implements HasColumnMappings {

    private final LayeredColumns columns = new LayeredColumns();
    protected final AttributeStore attributes;

    protected ColumnBasedMappingBase(AttributeStore underlyStore) {
        attributes = underlyStore.clone();
    }

    @Override
    public Collection<ColumnMapping> getColumns() {
        return columns.getColumns();
    }

    @Override
    public void addColumn(int layer, ColumnMapping column) {
        columns.addColumn(layer, column);
    }

    @Override
    public void makeColumnsEmpty(int layer) {
        columns.makeColumnsEmpty(layer);
    }
}
