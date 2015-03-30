package com.nd.demo.mapping;

import com.nd.demo.mapping.model.identity.GeneratorMapping;

import java.util.UUID;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping
 * @since 2015-03-30
 */
public class GeneratorBuilder {

    private final Class identityClazz;
    private final int layer;
    private final GeneratorMapping mapping;

    public GeneratorBuilder(GeneratorMapping mapping, Class identityClazz, int layer) {
        this.mapping = mapping;
        this.identityClazz = identityClazz;
        this.layer = layer;
    }

    private void setGenerator(String generator) {
        mapping.set(mapping.getClassName(), layer, generator);
    }

    private void addGeneratorParam(String name, String value) {
        mapping.getParams().put(name, value);
    }

    private void ensureIntegralIdentityType() {
        if(!isIntegralType(identityClazz)) {
            throw new UnsupportedOperationException("Identity type must be integral (int, long, uint, ulong)");
        }
    }

    private void ensureGuidIdentityType() {
        if(identityClazz != UUID.class) {
            throw new UnsupportedOperationException("Identity type must be Guid");
        }
    }

    private void ensureStringIdentityType() {
        if(identityClazz != String.class) {
            throw new UnsupportedOperationException("Identity type must be string");
        }
    }

    private static boolean isIntegralType(Class clazz) {

        if(clazz == Integer.class || clazz == Long.class ||
                clazz == Short.class || clazz == Byte.class) {
            return true;
        } else {
            return false;
        }
    }

    public void increment() {
        ensureIntegralIdentityType();
        setGenerator("increment");
    }

    public void identity() {
        ensureIntegralIdentityType();
        setGenerator("identity");
    }

    public void sequence(String sequenceName) {
        ensureIntegralIdentityType();
        setGenerator("sequence");
        addGeneratorParam("sequence", sequenceName);
    }

    public void guid() {
        ensureGuidIdentityType();
        setGenerator("guid");
    }

    public void uuidString()
    {
        ensureStringIdentityType();
        setGenerator("uuid.string");
    }
}
