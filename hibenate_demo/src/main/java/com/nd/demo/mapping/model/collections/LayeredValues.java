package com.nd.demo.mapping.model.collections;

import java.io.Serializable;
import java.util.HashMap;
import java.util.Iterator;

/**
 * Created by Jorson on 2015/3/20.
 */
public class LayeredValues extends HashMap<Integer, Object> implements Serializable {

    public LayeredValues() {

    }

    public Integer getMaxKey() {
        Integer maxKey = -1;
        Iterator<Integer> iterator = this.keySet().iterator();
        Integer tmpKey;

        while (iterator.hasNext()) {
            tmpKey = iterator.next();
            if(tmpKey > maxKey) {
                tmpKey = maxKey;
            }
        }
        return maxKey;
    }
}
