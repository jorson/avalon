package com.nd.demo.visitor;

import com.nd.demo.mapping.model.NaturalIdMapping;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.PropertyMapping;
import com.nd.demo.mapping.model.identity.GeneratorMapping;
import com.nd.demo.mapping.model.identity.IdMapping;
import com.nd.demo.mapping.model.identity.IdentityMapping;

import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.visitor
 * @since 2015-03-23
 */
public abstract class NullMappingModelVisitor implements MappingModelVisitor {

    @Override
    public void processId(IdMapping idMapping) {

    }

    @Override
    public void processClass(ClassMapping classMapping) {

    }

    @Override
    public void processColumn(ColumnMapping columnMapping) {

    }

    @Override
    public void processGenerator(GeneratorMapping generatorMapping) {

    }

    @Override
    public void processHibernateMapping(HibernateMapping hibernateMapping) {

    }

    @Override
    public void visit(IdMapping mapping) {

    }

    @Override
    public void visit(ClassMapping classMapping) {

    }

    @Override
    public void visit(ColumnMapping columnMapping) {

    }

    @Override
    public void visit(GeneratorMapping generatorMapping) {

    }

    @Override
    public void visit(List<HibernateMapping> mappings) {

    }

    @Override
    public void processProperty(PropertyMapping propertyMapping) {

    }

    @Override
    public void visit(PropertyMapping propertyMapping) {

    }

    @Override
    public void processNaturalId(NaturalIdMapping naturalIdMapping) {

    }

    @Override
    public void visit(NaturalIdMapping naturalIdMapping) {

    }

    @Override
    public void visit(IdentityMapping mapping) {

    }
}
