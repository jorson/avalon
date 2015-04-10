package com.nd.demo.config;

import com.nd.demo.PersistenceModel;
import org.hibernate.cfg.Configuration;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.config
 * @since 2015-04-08
 */
public class MappingConfiguration {

    private PersistenceModel model;

    private HbmMappingsContainer hbmMappingsContainer;
    private FluentMappingsContainer fluentMappingsContainer;

    public MappingConfiguration() {
        hbmMappingsContainer = new HbmMappingsContainer();
        fluentMappingsContainer = new FluentMappingsContainer();

        usePersistenceModel(new PersistenceModel());
    }

    public MappingConfiguration usePersistenceModel(PersistenceModel model) {
        this.model = model;
        return this;
    }

    public HbmMappingsContainer getHbmMappingsContainer() {
        return hbmMappingsContainer;
    }

    public FluentMappingsContainer getFluentMappingsContainer() {
        return fluentMappingsContainer;
    }

    public boolean isWasUse() {
        return hbmMappingsContainer.isWasUsed();
    }

    public void apply(Configuration cfg) {
        this.hbmMappingsContainer.apply(cfg);
        this.fluentMappingsContainer.apply(model);
        model.configure(cfg);
    }
}
