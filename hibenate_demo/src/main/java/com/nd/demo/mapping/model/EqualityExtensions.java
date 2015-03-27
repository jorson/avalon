package com.nd.demo.mapping.model;

import java.util.Collection;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model
 * @since 2015-03-26
 */
public final class EqualityExtensions {

    public static <K, V> boolean contentEquals(Map<K, V> left, Map<K, V> right) {
        if(left.size() != right.size()) {
            return false;
        }

        for(Map.Entry<K, V> item : left.entrySet()) {
            if(right.get(item.getKey()) == null) {
                return false;
            }

            V leftValue = item.getValue();
            V rightValue = right.get(item.getKey());
            if(!leftValue.equals(rightValue)) {
                return false;
            }
        }
        return true;
    }

    public static <T> boolean contentEquals(List<T> left, List<T> right) {
        if(left.size() != right.size()) {
            return false;
        }

        for(int i=0; i<left.size(); i++) {
            if(!left.get(i).equals(right.get(i))) {
                return false;
            }
        }
        return true;
    }
}
