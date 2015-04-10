package com.nd.demo.visitor;

import com.nd.demo.mapping.model.ColumnMapping;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.NaturalIdMapping;
import com.nd.demo.mapping.model.PropertyMapping;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.classbased.ClassMappingBase;
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
 * @since 2015-04-10
 */
public abstract class DefaultMappingModelVisitor extends NullMappingModelVisitor {

    protected void processIdentity(IdentityMapping identityMapping) {

    }

    @Override
    public void processId(IdMapping idMapping) {
        processIdentity(idMapping);
    }

    protected void processClassBase(ClassMappingBase classMapping) {

    }

    @Override
    public void processClass(ClassMapping classMapping) {
        processClassBase(classMapping);
    }

    @Override
    public void visit(List<HibernateMapping> mappings) {
        for(HibernateMapping mapping : mappings) {
            mapping.acceptVisitor(this);
        }
    }

    @Override
    public void visit(ClassMapping classMapping) {
        classMapping.acceptVisitor(this);
    }

    @Override
    public void visit(ColumnMapping columnMapping) {
        columnMapping.acceptVisitor(this);
    }

    @Override
    public void visit(GeneratorMapping generatorMapping) {
        generatorMapping.acceptVisitor(this);
    }

    @Override
    public void visit(PropertyMapping propertyMapping) {
        propertyMapping.acceptVisitor(this);
    }
}
