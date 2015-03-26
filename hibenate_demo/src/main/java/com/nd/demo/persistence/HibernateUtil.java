package com.nd.demo.persistence;

import org.hibernate.SessionFactory;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.service.ServiceRegistry;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;

/**
 * Created by Jorson on 2015/3/19.
 */
public class HibernateUtil {

    private static SessionFactory sessionFactory = buildSessionFactory();

    public static SessionFactory buildSessionFactory() {
        try{
            Configuration configuration = new Configuration();
            configuration.configure();

            String filePath = HibernateUtil.class.getClassLoader().getResource("mapping/UserEntry.hbm.xml").getPath();
            System.out.println("User mapping path is " + filePath);
            File userMapping = new File(filePath);
            BufferedReader bufferedReader = new BufferedReader(new FileReader(userMapping));
            String tmpLine = null;
            StringBuilder builder = new StringBuilder();
            while ((tmpLine = bufferedReader.readLine()) != null) {
                builder.append(tmpLine);
            }
            configuration.addXML(builder.toString());
            //configuration.addFile(userMapping);

            ServiceRegistry serviceRegistry = new StandardServiceRegistryBuilder()
                    .applySettings(configuration.getProperties())
                    .build();
            return configuration.buildSessionFactory(serviceRegistry);
        } catch (Exception ex) {
            throw new ExceptionInInitializerError(ex);
        }
    }

    public static SessionFactory getSessionFactory() {
        return sessionFactory;
    }

    public static void shutdown() {
        getSessionFactory().close();
    }
}
