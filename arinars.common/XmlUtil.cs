using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace arinars.common
{
    public sealed class XmlUtil
    {
        /// <summary>
        /// 해당 Element의 값을 가져온다.
        /// </summary>
        /// <param name="aXmlReader"></param>
        /// <param name="aRootElementName"></param>
        /// <param name="aElementName"></param>
        /// <returns></returns>
        public static string GetElementValue(XmlReader aXmlReader, string aRootElementName, string aElementName)
        {
            string lElementValue = null;
            while (aXmlReader.Read())
            {
                if (aXmlReader.IsStartElement())
                {
                    string lElementName = aXmlReader.LocalName;
                    if (lElementName.Equals(aElementName))
                    {
                        aXmlReader.Read();
                        lElementValue = aXmlReader.ReadString();
                        break;
                    }
                    else
                    {
                        //ignore
                    }
                }
                if (aXmlReader.NodeType == XmlNodeType.EndElement)
                {
                    string lElementName = aXmlReader.LocalName;
                    if (lElementName.Equals(aRootElementName)) //root element
                    {
                        break;
                    }
                }
            }
            aXmlReader.Close();
            return lElementValue;
        }

        /// <summary>
        /// 해당 노드의 값을 가져온다.
        /// </summary>
        /// <param name="aDoc"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string GetNodeValue(XmlDocument aDoc, string xpath)
        {

            XmlNode lCurrentVersionNode = null;
            foreach (XmlNode lNode in aDoc.SelectNodes(xpath))
            {
                lCurrentVersionNode = lNode;
                break;
            }
            return lCurrentVersionNode != null ? lCurrentVersionNode.InnerText : null;
        }

        /// <summary>
        /// 해당 노드의 값을 가져온다.
        /// </summary>
        /// <param name="aDoc"></param>
        /// <param name="xpath"></param>
        /// <param name="aKeyAttr"></param>
        /// <param name="aValueAttr"></param>
        /// <returns></returns>
        public static string GetNodeValue(XmlDocument aDoc, string xpath, string aKeyAttr, string aValueAttr)
        {
            XmlNode lVersionNode = null;
            foreach (XmlNode lNode in aDoc.SelectNodes(xpath))
            {
                if (lNode.Attributes[aKeyAttr].InnerText == aValueAttr)
                {
                    lVersionNode = lNode;
                    break;
                }
            }
            return lVersionNode != null ? lVersionNode.InnerText : null;
        }
    }
}
