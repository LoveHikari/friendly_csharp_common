using System.Xml;

namespace Hikari.Common.Xml
{
    /// <summary>
    /// <see cref="XmlDocument"/> 帮助类
    /// </summary>
    public class XmlDocumentHelper
    {
        private System.Xml.XmlDocument _xml;
        private string? _filePath;
        /// <summary>
        /// 从指定的 URL 加载 XML 文档。
        /// </summary>
        /// <param name="filePath">含要加载的 XML 文档的文件的 URL。
        ///    URL 既可以是本地文件，也可以是 HTTP URL（Web 地址）。</param>
        public XmlDocumentHelper(string filePath)
        {
            this._filePath = filePath;

            this._xml = new XmlDocument();
            this._xml.Load(this._filePath);
        }

        /// <summary>
        /// 从指定的 URL 加载 XML 文档。
        /// </summary>
        /// <param name="inStream">含要加载的 XML 文档的文件的 URL。URL 既可以是本地文件，也可以是 HTTP URL（Web 地址）。</param>
        public XmlDocumentHelper(System.IO.Stream inStream)
        {
            this._xml = new XmlDocument();
            this._xml.Load(inStream);
        }
        /// <summary>
        /// 获得节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public string GetSingleNode(string xPath)
        {
            XmlNode? xmlNode = _xml.SelectSingleNode(xPath);
            if (xmlNode != null)
            {
                return xmlNode.InnerText;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 设置节点，如果不存在则添加
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="value"></param>
        public void SetSingleNode(string xPath, string value)
        {
            XmlNode? xmlNode = _xml.SelectSingleNode(xPath);
            if (xmlNode == null)
            {
                string[] ss = xPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ss)
                {
                    _xml.CreateElement(s);
                }
                xmlNode = _xml.SelectSingleNode(xPath);
            }
            if (xmlNode != null) xmlNode.InnerText = value;

        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string? filePath = null)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                _xml.Save(filePath);
            } else if (!string.IsNullOrWhiteSpace(_filePath))
            {
                _xml.Save(_filePath);
            }
            else
            {
                throw new Exception("文件路径为空");
            }
                
        }
    }
}