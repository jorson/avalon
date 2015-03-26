package com.nd.demo.visitor;

import com.nd.demo.mapping.model.ClassMapping;
import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.identity.GeneratorMapping;
import com.nd.demo.mapping.model.identity.IdMapping;

import java.util.Enumeration;
import java.util.Iterator;

/**
 * Created by Jorson on 2015/3/20.
 */
public interface MappingModelVisitor {

    void processId(IdMapping idMapping);
    void processClass(ClassMapping classMapping);
    void processColumn(ColumnMapping columnMapping);
    void processGenerator(GeneratorMapping generatorMapping);
    void processHibernateMapping(HibernateMapping hibernateMapping);

    void visit(Iterator<HibernateMapping> mappings);

    void visit(IdMapping mapping);
    void visit(ClassMapping classMapping);
    void visit(ColumnMapping columnMapping);
    void visit(GeneratorMapping generatorMapping);
}
