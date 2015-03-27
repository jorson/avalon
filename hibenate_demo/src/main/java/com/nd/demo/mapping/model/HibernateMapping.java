package com.nd.demo.mapping.model;

import com.nd.demo.mapping.MappingBase;
import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.visitor.MappingModelVisitor;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Jorson on 2015/3/20.
 */
public class HibernateMapping extends MappingBase {

    private final List<ClassMapping> classMappings;
    private final AttributeStore attributeStore;

    public HibernateMapping() {
        this(new AttributeStore());
    }

    public HibernateMapping(AttributeStore attributes) {
        this.attributeStore = attributes;
        classMappings = new ArrayList<ClassMapping>();
    }

    @Override
    public void acceptVisitor(MappingModelVisitor visitor) {
        visitor.processHibernateMapping(this);

        for(ClassMapping mapping : classMappings) {
            visitor.visit(mapping);
        }
    }

    @Override
    public boolean isSpecified(String attribute) {
        return attributeStore.isSpecified(attribute);
    }

    @Override
    public void set(String attribute, int layer, Object value) {
        attributeStore.set(attribute, layer, value);
    }

    public void addClass(ClassMapping classMapping) {
        this.classMappings.add(classMapping);
    }

    public String getCatalog()
    {
        return attributeStore.getOrDefault("Catalog");
    }

    public String getDefaultAccess()
    {
        return attributeStore.getOrDefault("DefaultAccess");
    }

    public String getDefaultCascade()
    {
        return attributeStore.getOrDefault("DefaultCascade");
    }

    public boolean getAutoImport()
    {
        return attributeStore.getOrDefault("AutoImport");
    }

    public String getSchema()
    {
        return attributeStore.getOrDefault("Schema");
    }

    public boolean getDefaultLazy()
    {
        return attributeStore.getOrDefault("DefaultLazy");
    }

    public String getNamespace()
    {
        return attributeStore.getOrDefault("Namespace");
    }

    public String getAssembly()
    {
        return attributeStore.getOrDefault("Assembly");
    }


    public List<ClassMapping> getClassMappings() {
        return classMappings;
    }


    @Override
    public int hashCode() {
        return super.hashCode();
    }

    @Override
    public boolean equals(Object obj) {
        return super.equals(obj);
    }
}
