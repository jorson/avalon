package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.writer.sorting.XmlNodeSorter;
import com.nd.demo.visitor.NullMappingModelVisitor;
import org.dom4j.Document;
import org.dom4j.DocumentFactory;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.dom4j.dom.DOMDocumentFactory;
import org.dom4j.dom.DOMNodeHelper;

import static com.nd.demo.mapping.model.writer.XmlExtensions.*;

/**
 * 在这里输入标题
 * <p/>
 * 说明
 *
 * @author jorson.WHY
 * @package com.nd.demo.mapping.model.writer
 * @since 2015-03-26
 */
public class XmlHibernateMappingWriter extends NullMappingModelVisitor
        implements XmlWriter<HibernateMapping> {

    private final XmlWriterServiceLocator serviceLocator;
    private Document document;

    public XmlHibernateMappingWriter(XmlWriterServiceLocator serviceLocator) {
        this.serviceLocator = serviceLocator;
    }

    public Document write(HibernateMapping mapping) {
        mapping.acceptVisitor(this);
        return document;
    }

    @Override
    public void processHibernateMapping(HibernateMapping hibernateMapping) {
        //创建Document对象
        document = DocumentHelper.createDocument();
        //创建hbm的根节点
        Element element = document.addElement("hibernate-mapping");
        document.setRootElement(element);
        //设置必要的属性
        withAttr(element, "xmlns", "urn:nhibernate-mapping-2.2");
        //设置可选的属性
        if(hibernateMapping.isSpecified("DefaultAccess")) {
            withAttr(element, "default-access", hibernateMapping.getDefaultAccess());
        }
        if(hibernateMapping.isSpecified("AutoImport")) {
            withAttr(element, "auto-import", hibernateMapping.getDefaultAccess());
        }
        if(hibernateMapping.isSpecified("Schema")) {
            withAttr(element, "schema", hibernateMapping.getSchema());
        }
        if(hibernateMapping.isSpecified("DefaultCascade")) {
            withAttr(element, "default-cascade", hibernateMapping.getDefaultCascade());
        }
        if(hibernateMapping.isSpecified("DefaultLazy")) {
            withAttr(element, "default-lazy", hibernateMapping.getDefaultLazy());
        }
        if(hibernateMapping.isSpecified("Catalog")) {
            withAttr(element, "catalog", hibernateMapping.getCatalog());
        }
        if(hibernateMapping.isSpecified("Assembly")) {
            withAttr(element, "package", hibernateMapping.getPackage());
        }
    }

    @Override
    public void visit(ClassMapping classMapping) {
        //TODO: not sort
        XmlWriter<ClassMapping> writer = serviceLocator.getWriter(ClassMapping.class);
        Document hbmClass = writer.write(classMapping);
        Element hbmElement = hbmClass.getRootElement();
        document.getRootElement().add(hbmElement);
    }
}
