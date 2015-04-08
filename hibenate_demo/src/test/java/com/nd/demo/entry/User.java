package com.nd.demo.entry;

import java.util.Date;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.entry
 * @since 2015-03-31
 */
public class User {

    private Integer id;
    private String name;
    private Integer age;
    private Date birthday;


    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Integer getAge() {
        return age;
    }

    public void setAge(Integer age) {
        this.age = age;
    }

    public Date getBirthday() {
        return birthday;
    }

    public void setBirthday(Date birthday) {
        this.birthday = birthday;
    }
}
