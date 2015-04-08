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

    public MappingConfiguration() {
        hbmMappingsContainer = new HbmMappingsContainer();
    }

    public MappingConfiguration usePersistenceModel(PersistenceModel model) {
        this.model = model;
        return this;
    }

    public HbmMappingsContainer getHbmMappingsContainer() {
        return hbmMappingsContainer;
    }

    public boolean isWasUse() {
        return hbmMappingsContainer.isWasUsed();
    }

    public void apply(Configuration cfg) {
        this.hbmMappingsContainer.apply(cfg);
        model.configure(cfg);
    }
}
