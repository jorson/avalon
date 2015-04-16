package com.nd.demo.infrastructure;

import com.nd.demo.mapping.model.writer.XmlWriter;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.infrastructure
 * @since 2015-03-23
 */
public interface Instantiation {

    public Object registeredType(Container container);
}
