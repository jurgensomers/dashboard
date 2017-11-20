using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;
using Dashquoia.Api.Models;
using Newtonsoft.Json;

namespace Dashquoia.Api.Converters
{
    public class IsAliveResponseConverter
    {
        public StatusType Convert(Message response, Exception exception)
        {
            if (exception == null)
                return ConvertSuccess(response);
            return ConvertException(exception);
        }

        private StatusType ConvertSuccess(Message response)
        {
            using (XmlDictionaryReader readerAtBodyContents = response.GetReaderAtBodyContents())
            {
                var result = readerAtBodyContents.ReadOuterXml();

                XElement body = XElement.Parse(result);

                if (body.Descendants().Any(d => d.Name.LocalName == "IsAliveResult"))
                {
                    XElement soapResponse = body.Descendants().First(d => d.Name.LocalName == "IsAliveResult");
                    if (soapResponse.Value == "true")
                    {
                        var xmlResult = new XElement("IsAlive", new XElement("IsAliveResult", "true"));
                        var jsonResult = JsonConvert.SerializeXNode(xmlResult);
                        dynamic typedResult = JsonConvert.DeserializeObject(jsonResult);
                        return (bool) typedResult.IsAlive.IsAliveResult ? StatusType.Up : StatusType.Down;
                    }
                }
                return StatusType.Unknown;
            }
        }

        private StatusType ConvertException(Exception exc)
        {
            if ( exc is EndpointNotFoundException )
                return StatusType.Down; // --> something wrong in the configuration
            else 
                return StatusType.Down;
        }
    }
}