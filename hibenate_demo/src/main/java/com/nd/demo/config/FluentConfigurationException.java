package com.nd.demo.config;

import java.util.ArrayList;
import java.util.List;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.config
 * @since 2015-04-09
 */
public class FluentConfigurationException extends Exception {

    private final List<String> potentialReasons;

    public FluentConfigurationException(String message, Exception innerException) {
        super(message, innerException);
        potentialReasons = new ArrayList<String>();
    }

    public List<String> getPotentialReasons() {
        return potentialReasons;
    }

    @Override
    public String getMessage() {
        String lineSeparator = System.getProperty("line.separator", "\n");
        String output = super.getMessage();
        output += lineSeparator + lineSeparator;

        for(String reason : potentialReasons) {
            output += " * " + reason;
            output += lineSeparator;
        }

        return output;
    }

    @Override
    public String toString() {
        String lineSeparator = System.getProperty("line.separator", "\n");
        String output = super.getMessage();
        output += lineSeparator + lineSeparator;

        for(String reason : potentialReasons) {
            output += " * " + reason;
            output += lineSeparator;
        }

        return output;
    }
}
