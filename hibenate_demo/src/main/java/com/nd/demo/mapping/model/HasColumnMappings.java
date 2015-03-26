package com.nd.demo.mapping.model;

import java.util.Iterator;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-26
 */
public interface HasColumnMappings {

    Iterator<ColumnMapping> getColumns();
    void addColumn(int layer, ColumnMapping column);
    void makeColumnsEmpty(int layer);
}
