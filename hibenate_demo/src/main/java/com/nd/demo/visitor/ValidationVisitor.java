package com.nd.demo.visitor;

import com.nd.demo.mapping.model.classbased.ClassMapping;

import javax.xml.bind.ValidationException;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.visitor
 * @since 2015-04-14
 */
public class ValidationVisitor extends DefaultMappingModelVisitor {

    private boolean enabled = true;

    public ValidationVisitor() {
        this.enabled = true;
    }

    @Override
    public void processClass(ClassMapping classMapping) {
        if(!this.enabled) {
            return;
        }

        if(classMapping.getId() == null) {
            throw new IllegalArgumentException(String.format("The entity '%s' doesn't have an Id mapped.",
                    classMapping.getClazz().getName()));
        }
    }

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }
}
