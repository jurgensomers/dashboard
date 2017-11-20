using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Xml;
using Dashquoia.Api.Configuration;

namespace Dashquoia.Api.Managers
{
    public class ServiceAgent
    {
        public TResult Send<TResult>(string body, Setting setting, Func<Message, Exception, TResult> converter)
        {
            using (Stream requestStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    try
                    {
                        writer.Write(body);
                        writer.Flush();
                        requestStream.Position = 0;
                        var xReader = XmlReader.Create(requestStream);

                        var binding = CreateBinding(setting.BindingName, setting.BindingType);

                        Message request = Message.CreateMessage(binding.MessageVersion, setting.Action, xReader);
                        ChannelFactory<IRequestChannel> factory = CreateChannelFactory(binding, setting.Service, setting.Identity);

                        var channel = factory.CreateChannel();
                        var message = channel.Request(request);
                        return converter(message, null);
                    }
                    catch (Exception exc)
                    {
                        //Log.Error($"Error while trying to execute IsAlive on {setting.Environment}-{setting.Service}");
                        return converter(null, exc);
                    }
                }
            }
        }

        private ChannelFactory<IRequestChannel> CreateChannelFactory(Binding binding, string address, string identity)
        {
            var factory = new ChannelFactory<IRequestChannel>(binding, CreateAddress(binding, address, identity));
            if (binding is NetTcpBinding) factory.Endpoint.Contract.SessionMode = SessionMode.Allowed;
            return factory;
        }

        private EndpointAddress CreateAddress(Binding binding, string address, string identity)
        {
            if (binding is NetTcpBinding && !string.IsNullOrWhiteSpace(identity))
            {
                var endpointIdentity = EndpointIdentity.CreateUpnIdentity(identity);
                return new EndpointAddress(new Uri(address), endpointIdentity);
            }
            return new EndpointAddress(new Uri(address));
        }

        private Binding CreateBinding(string bindingName, string bindingType)
        {
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            var element = serviceModel.Bindings[bindingType];
            Binding binding = null;
            if (!string.IsNullOrWhiteSpace(bindingName))
                binding = (Binding)Activator.CreateInstance(element.BindingType, bindingName);
            else
                binding = (Binding)Activator.CreateInstance(element.BindingType);

            binding.CloseTimeout = new TimeSpan(0, 0, 30);
            binding.OpenTimeout = new TimeSpan(0, 0, 30);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = new TimeSpan(0, 0, 30);

            return binding;
        }
    }
}