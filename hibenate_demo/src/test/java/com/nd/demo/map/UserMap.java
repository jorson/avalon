package com.nd.demo.map;

import com.nd.demo.entry.User;
import com.nd.demo.mapping.ClassMap;
import com.nd.demo.mapping.MappingProviderStore;
import com.nd.demo.mapping.model.AttributeStore;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.map
 * @since 2015-03-31
 */
public class UserMap extends ClassMap<User> {

    public UserMap() {
        table("demo_user");
        id(getField("id"));
        map(getField("name"), "user_name");
        map(getField("age"), "user_age");
    }
}
