package com.nd.demo;

import com.nd.demo.entry.UserEntry;
import com.nd.demo.persistence.HibernateUtil;
import org.hibernate.Session;

/**
 * Created by Jorson on 2015/3/19.
 */
public class DemoMain {

    public static void main(String[] args) {
        System.out.println("This is Demo Main!");

        Session session = HibernateUtil.getSessionFactory().openSession();
        session.beginTransaction();

        UserEntry user = new UserEntry();
        user.setName("AAA");
        user.setAge(12);
        session.save(user);
        session.getTransaction().commit();

        System.exit(0);
    }
}
