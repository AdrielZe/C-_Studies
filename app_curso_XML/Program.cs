using app_curso_XML.MySite.Common.BizTalkUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XMLDocument_Demo2
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlInputFilename = @"C:\Users\adrie\Desktop\AULAS XML 2024\IntroSamples\Flight03.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlInputFilename);

            Console.WriteLine("\n\n File= " + xmlInputFilename);
            Console.WriteLine(xmlDoc.OuterXml);

          
          

            string formattedXML = XmlFormatter.PrettyPrint(xmlDoc);

            Console.WriteLine(xmlDoc.OuterXml);
            Console.WriteLine("\n\n");
            Console.WriteLine(formattedXML);

            Console.WriteLine("\n\n File= " + xmlInputFilename);
           

            //How to select a single node:
            Console.WriteLine("\n\n");
            string xpath = "/AirlineName";

            XmlNode node = xmlDoc.SelectSingleNode(xpath);

            Console.WriteLine("Node Value: " + GetNodeValue(node));

            //How to select multiple nodes:
            string xpathMultiple = "//DepartureAirport";
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpathMultiple);

            Console.WriteLine("Number of Matching Nodes = " + xmlNodeList.Count);
            Console.WriteLine("Querie for Xpath: " + xpathMultiple);

            foreach(XmlNode nodeX in xmlNodeList)
            {
                Console.WriteLine("Node = " + GetNodeValue(nodeX));
            }


            //How to select an element by Tag Name:
            XmlNodeList nodeList2 = xmlDoc.GetElementsByTagName("DepartureAirport");
            //This is the same as using xPath with the // at the beginning of the node name.


            //How to navigate between node.
            //Navigation Methods: parentNode, childNodes, firstChild, lastChild, nextSibling, previousSibling
            string xpathNav = "//DepartureAirport";
         
            XmlNode nodeNav1 = xmlDoc.SelectSingleNode(xpathNav);

            Console.WriteLine("nodeNav1= " + GetNodeValue(nodeNav1) + " for xPath " + xpathNav);
              
            XmlNode nextSiblingNode = nodeNav1.NextSibling;
            Console.WriteLine("nextSiblingNode.Name = " + nextSiblingNode.Name + " Value = " +
                GetNodeValue(nextSiblingNode));


            XmlNode prevSiblingNode = nodeNav1.PreviousSibling;
            Console.WriteLine("prevSiblingSiblingNode.Name = " + prevSiblingNode.Name + " Value = " +
               GetNodeValue(prevSiblingNode));

            XmlNode parentNode = nodeNav1.ParentNode;
            Console.WriteLine("parentNode.Name = " + parentNode.Name + " Value = " + GetNodeValue(parentNode));

            if (parentNode.NodeType.ToString() == "Element")
            {
                XmlElement parentElement = (XmlElement)parentNode;
                Console.WriteLine("FlightLeg/@seq= " + parentElement.GetAttribute("seq"));
            }
            else
            {
                Console.WriteLine("Expecting parent of DepartureAirport to ben an element but it was not.");
            }

            //How to validate XML data against and XSD Schema
            string schemaFilename = @"C:\Users\adrie\Desktop\AULAS XML 2024\IntroSamples\Flight03_dotNet.xsd";
            string validXMLFilename = @"C:\Users\adrie\Desktop\AULAS XML 2024\IntroSamples\Flight03Min.xml";
            string validationResult = ValidateXMLFileAgainstSchema(validXMLFilename, schemaFilename);
            Console.WriteLine("First Validation Result: " + validationResult);


            string invalidXMLFilename = @"C:\Users\adrie\Desktop\AULAS XML 2024\IntroSamples\Flight03_Bad.xml";
            validationResult = ValidateXMLFileAgainstSchema(invalidXMLFilename,schemaFilename);
            Console.WriteLine("result: " + validationResult);

            //How to add an atribute called 'Direct' with value "yes" or "no" to the first Flight
            Console.WriteLine("Adding new 'direct' atribute to first Flight");
            string xpathFlight1 = "//Flight";
            XmlNodeList nodeListFlight1 = xmlDoc.SelectNodes(xpathFlight1);

            foreach(XmlNode flightNode in nodeListFlight1)
            {
                string varYesNo = "yes";

                string xPathFlightLegForCount = "FlightLeg";
                XmlNodeList nodeListFlightLegs = flightNode.SelectNodes(xPathFlightLegForCount);
                if (nodeListFlightLegs.Count > 1)
                {
                    varYesNo = "no";
                }

                ((XmlElement)flightNode).SetAttribute("direct", varYesNo);
            }

            //string formattedXML3 = XmlFormatter.PrettyPrint(xmlDoc);
            //Console.WriteLine(formattedXML3);


            //How to add an element called 'SeatAssignment' with value 'temp'
            Console.WriteLine("Adding new 'SeatAssignment' atribute");
            string xpathFlightLeg1 = "//FlightLeg";
            XmlNodeList nodeListFlightLeg1 = xmlDoc.SelectNodes(xpathFlightLeg1);

            foreach (XmlNode flightLegNode in nodeListFlightLeg1)
            {
                XmlElement newSeatElement = xmlDoc.CreateElement("SeatAssignment");
                newSeatElement.InnerText = "temp";
                flightLegNode.AppendChild(newSeatElement);
            }

            string formattedXML3 = XmlFormatter.PrettyPrint(xmlDoc);
            Console.WriteLine(formattedXML3);





        }
        static string GetNodeValue(XmlNode nodeArg)
        {
            if (nodeArg ==null)
            {
                return "";
            }
            else
            {
                return nodeArg.FirstChild.Value;
            }
        }

        static string ValidateXMLFileAgainstSchema(string xmlPath, string xsdPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);

            xml.Schemas.Add(null, xsdPath);

            try
            {
                xml.Validate(null);
                return "Success";
            }
            catch (XmlSchemaValidationException ex)
            {
                Console.WriteLine("Second validation: ValidationError: ");
                return ex.Message;
            }
        }
    }
}

    