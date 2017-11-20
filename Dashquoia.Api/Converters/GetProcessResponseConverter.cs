using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Linq;

namespace Dashquoia.Api.Managers
{
    public class GetProcessResponseConverter
    {
        public List<dynamic> Convert(Message response, Exception exception)
        {
            if (exception != null)
            {
                return null;
            }

            var list = new List<dynamic>();
            using (XmlDictionaryReader readerAtBodyContents = response.GetReaderAtBodyContents())
            {
                var result = readerAtBodyContents.ReadOuterXml();

                XElement body = XElement.Parse(result);

                if (body.Descendants().Any(d => d.Name.LocalName == "GetProcessesResult"))
                {
                    var processInformations = body.Descendants().Where(d => d.Name.LocalName == "ProcessInformation");
                    foreach (var processInformation in processInformations)
                    {
                        var identifier = processInformation.Descendants().FirstOrDefault(d => d.Name.LocalName == "Identifier")?.Value;
                        var processType = processInformation.Descendants().FirstOrDefault(d => d.Name.LocalName == "ProcessType")?.Value;
                        var state = processInformation.Descendants().FirstOrDefault(d => d.Name.LocalName == "State")?.Value;
                        var toggleMode = processInformation.Descendants().FirstOrDefault(d => d.Name.LocalName == "ToggleMode")?.Value;
                        var item = new
                        {
                            Identifier = string.Join("_", identifier, toggleMode),
                            ProcessType = processType,
                            State = state
                        };
                        list.Add(item);
                    }
                }
            }

            return list;
        }
    }
}