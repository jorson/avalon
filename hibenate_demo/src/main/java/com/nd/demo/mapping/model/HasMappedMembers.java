package com.nd.demo.mapping.model;

import java.util.Collection;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-26
 */
public interface HasMappedMembers {

    Collection<PropertyMapping> getProperties();

    void addProperty(PropertyMapping property);
}
