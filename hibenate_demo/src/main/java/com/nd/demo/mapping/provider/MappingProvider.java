package com.nd.demo.mapping.provider;

import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.classbased.ClassMapping;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.provider
 * @since 2015-03-30
 */
public interface MappingProvider {

    ClassMapping getClassMapping();
    HibernateMapping getHibernateMapping();
}
