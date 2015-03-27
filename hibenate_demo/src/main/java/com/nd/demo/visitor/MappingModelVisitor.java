package com.nd.demo.visitor;

import com.nd.demo.mapping.model.*;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.identity.GeneratorMapping;
import com.nd.demo.mapping.model.identity.IdMapping;

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
    void processProperty(PropertyMapping propertyMapping);
    void processNaturalId(NaturalIdMapping naturalIdMapping);

    void visit(Iterator<HibernateMapping> mappings);

    void visit(IdMapping mapping);
    void visit(ClassMapping classMapping);
    void visit(ColumnMapping columnMapping);
    void visit(GeneratorMapping generatorMapping);
    void visit(PropertyMapping propertyMapping);
    void visit(NaturalIdMapping naturalIdMapping);
}
