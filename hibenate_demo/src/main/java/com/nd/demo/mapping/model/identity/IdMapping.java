package com.nd.demo.mapping.model.identity;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.mapping.model.TypeReference;
import com.nd.demo.visitor.MappingModelVisitor;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-23
 */
public class IdMapping extends MappingBase {
    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {

    }

    @Override
    public boolean isSpecified(String attribute) {
        return false;
    }

    @Override
    public void set(String attribute, int layer, Object value) {

    }

    public String getAccess() {
        return null;
    }

    public String getName() {
        return null;
    }

    public TypeReference getType() {
        return null;
    }

    public String getUnsavedValue() {
        return null;
    }
}
