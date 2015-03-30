package com.nd.demo.mapping;

import com.nd.demo.mapping.provider.IdentityMappingProvider;
import com.nd.demo.mapping.provider.PropertyMappingProvider;

import java.util.ArrayList;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-30
 */
public class MappingProviderStore {

    private List<PropertyMappingProvider> properties;
    private IdentityMappingProvider id;

    public MappingProviderStore() {
        properties = new ArrayList<PropertyMappingProvider>();
    }

    public List<PropertyMappingProvider> getProperties() {
        return properties;
    }

    public void setProperties(List<PropertyMappingProvider> properties) {
        this.properties = properties;
    }

    public IdentityMappingProvider getId() {
        return id;
    }

    public void setId(IdentityMappingProvider id) {
        this.id = id;
    }
}
