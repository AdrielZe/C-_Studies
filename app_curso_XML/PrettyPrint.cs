using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace app_curso_XML
{
    namespace MySite.Common.BizTalkUtil
    {
        public static class XmlFormatter
        {
            // Neal Walters - added 09/11/2015 from 
            // http://stackoverflow.com/questions/203528/what-is-the-simplest-way-to-get-indented-xml-with-line-breaks-from-xmldocument
            public static string PrettyPrint(this XmlDocument doc)
            {
                var stringWriter = new StringWriter(new StringBuilder());
                var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
                doc.Save(xmlTextWriter);
                return stringWriter.ToString();
            }
        }
    }
}