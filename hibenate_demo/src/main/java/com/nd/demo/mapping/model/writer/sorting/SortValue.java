package com.nd.demo.mapping.model.writer.sorting;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer.sorting
 * @since 2015-03-26
 */
public class SortValue {

    private int position;
    private int level;

    public SortValue(int position, int level) {
        this.position = position;
        this.level = level;
    }

    public int getPosition() {
        return position;
    }

    public void setPosition(int position) {
        this.position = position;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }
}
