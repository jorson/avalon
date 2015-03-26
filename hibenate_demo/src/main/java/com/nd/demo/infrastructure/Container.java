package com.nd.demo.infrastructure;

import java.util.HashMap;
import java.util.Map;

/**
 * @author jorson.WHY
 * @package com.nd.demo.infrastructure
 * @since 2015-03-23
 */
public class Container {

    private final Map<Class, Instantiation> registeredTypes =
            new HashMap<Class, Instantiation>();

    public void register(Instantiation instantiationFunc, Class registerClazz) {
        if(!registeredTypes.containsKey(registerClazz)) {
            registeredTypes.put(registerClazz, instantiationFunc);
        }
    }

    public Object resolve(Class clazz) throws ResolveException {
        if(!registeredTypes.containsKey(clazz)) {
            throw new ResolveException(clazz);
        }
        Instantiation instantiationFunc = registeredTypes.get(clazz);
        return instantiationFunc.registeredType(this);
    }
}
