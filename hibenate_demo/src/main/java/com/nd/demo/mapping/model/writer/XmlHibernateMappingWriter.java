package com.nd.demo.mapping.model.writer;

import com.nd.demo.mapping.model.classbased.ClassMapping;
import com.nd.demo.mapping.model.HibernateMapping;
import com.nd.demo.mapping.model.writer.sorting.XmlNodeSorter;
import com.nd.demo.visitor.NullMappingModelVisitor;
import org.dom4j.dom.DOMDocument;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;

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
        document = new DOMDocument();

        Element element = document.createElement("hibernate-mapping");
        withAttr(element, "xmlns", "urn:nhibernate-mapping-2.2");

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
        XmlWriter<ClassMapping> writer = serviceLocator.getWriter();
        Document hbmClass = writer.write(classMapping);
        Node newClassNode = document.importNode(hbmClass.getDocumentElement(), true);
        XmlNodeSorter.sortClassChildren(newClassNode);

        document.getDocumentElement().appendChild(newClassNode);
    }
}
