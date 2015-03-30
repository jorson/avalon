package com.nd.demo.mapping;

import com.nd.demo.mapping.model.Layer;
import com.nd.demo.mapping.model.identity.GeneratorMapping;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-30
 */
public class IdentityGenerationStrategyBuilder<T> {

    private final T parent;
    private final Class entity;
    private final GeneratorMapping mapping = new GeneratorMapping();
    private final GeneratorBuilder builder;

    private boolean isDirty;

    public IdentityGenerationStrategyBuilder(T parent, Class identityType, Class entity) {
        this.parent = parent;
        this.entity = entity;
        builder = new GeneratorBuilder(mapping, identityType, Layer.USER_SUPPLIED);
    }

    public GeneratorMapping getGeneratorMapping() {
        mapping.setContainingEntityType(entity);
        return mapping;
    }

    public T increment() {
        builder.increment();
        isDirty = true;
        return parent;
    }

    public T identity() {
        builder.identity();
        isDirty = true;
        return parent;
    }

    public T uuidString() {
        builder.uuidString();
        isDirty = true;
        return parent;
    }

    public T guid() {
        builder.guid();
        isDirty = true;
        return parent;
    }

    public boolean isDirty() {
        return isDirty;
    }
}
