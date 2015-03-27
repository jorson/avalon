package com.nd.demo.mapping.model.classbased;

import com.nd.demo.mapping.model.AttributeStore;
import com.nd.demo.mapping.model.NaturalIdMapping;
import com.nd.demo.mapping.model.classbased.ClassMappingBase;
import com.nd.demo.mapping.model.identity.IdentityMapping;

import java.io.Serializable;

public class ClassMapping extends ClassMappingBase {

    public final AttributeStore attributeStore;

    public ClassMapping() {
        this(new AttributeStore());
    }

    public ClassMapping(AttributeStore attributes) {
        super(attributes);
        this.attributeStore = attributes;
    }

    public IdentityMapping getId() {
        return attributeStore.getOrDefault("Id");
    }

    public NaturalIdMapping getNaturalId() {
        return attributeStore.getOrDefault("NaturalId");
    }

    public String getName() {
        return attributeStore.getOrDefault("Name");
    }

    public Class getClazz() {
        return attributeStore.getOrDefault("Type");
    }

    public String getTableName() {
        return attributeStore.getOrDefault("TableName");
    }

    public int getBatchSize() {
        return attributeStore.getOrDefault("BatchSize");
    }

    public Object getDiscriminatorValue() {
        return attributeStore.getOrDefault("DiscriminatorValue");
    }

    public String getSchema() {
        return attributeStore.getOrDefault("Schema");
    }

    public boolean getLazy() {
        return attributeStore.getOrDefault("Lazy");
    }

    public boolean getMutable() {
        return attributeStore.getOrDefault("Mutable");
    }

    public boolean getDynamicUpdate() {
        return attributeStore.getOrDefault("DynamicUpdate");
    }

    public boolean getDynamicInsert() {
        return attributeStore.getOrDefault("DynamicInsert");
    }

    public String getOptimisticLock() {
        return attributeStore.getOrDefault("OptimisticLock");
    }

    public String getPolymorphism() {
        return attributeStore.getOrDefault("Polymorphism");
    }

    public String getPersister() {
        return attributeStore.getOrDefault("Persister");
    }

    public String getWhere() {
        return attributeStore.getOrDefault("Where");
    }

    public String getCheck() {
        return attributeStore.getOrDefault("Check");
    }

    public String getProxy() {
        return attributeStore.getOrDefault("Proxy");
    }

    public boolean getSelectBeforeUpdate() {
        return attributeStore.getOrDefault("SelectBeforeUpdate");
    }

    public boolean getAbstract() {
        return attributeStore.getOrDefault("Abstract");
    }

    public String getSubselect() {
        return attributeStore.getOrDefault("Subselect");
    }

    public String getSchemaAction()  {
        return attributeStore.getOrDefault("SchemaAction");
    }

    public String getEntityName() {
        return attributeStore.getOrDefault("EntityName");
    }

    @Override
    public boolean isSpecified(String attribute) {
        return attributeStore.isSpecified(attribute);
    }

    @Override
    public void set(String attribute, int layer, Object value) {
        attributeStore.set(attribute, layer, value);
    }
}
