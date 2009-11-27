namespace NDDDSample.Interfaces.HandlingService.Host.IoC
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Infrastructure.Messaging;

    #endregion

    public class MessageHandlerRegister
    {
        public MessageHandlerRegister(IWindsorContainer container, Assembly messagesAssembly, Assembly handlersAssembly)
        {          
            var messages = GetMessages(messagesAssembly);
            var messageHandlers = GetMessageHandlers(handlersAssembly);

            var stringBuilder = new StringBuilder();
            foreach (var message in messages)
            {
                if (!messageHandlers.ContainsKey(message))
                {
                    stringBuilder.AppendLine(string.Format("No message handler found for message '{0}'",
                                                           message.FullName));
                    continue;
                }

                foreach (var messageHandler in messageHandlers[message])
                {
                    Type genericType = typeof (IMessageHandler<>).MakeGenericType(message);
                    container.Register(Component.For(genericType)
                                           .ImplementedBy(messageHandler));
                }
            }
            if (stringBuilder.Length > 0)
            {
                throw new Exception(string.Format("\n\nMessage handler exceptions:\n{0}\n", stringBuilder));
            }
        }


        private static IDictionary<Type, IList<Type>> GetMessageHandlers(Assembly messageHandleAssembly)
        {
            IDictionary<Type, IList<Type>> messages = new Dictionary<Type, IList<Type>>();
            messageHandleAssembly
                .GetExportedTypes()
                .Where(
                x =>
                x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof (IMessageHandler<>)))
                .ToList()
                .ForEach(x => AddItem(messages, x));
            return messages;
        }

        private static void AddItem(IDictionary<Type, IList<Type>> dictionary, Type type)
        {
            var message = type.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IMessageHandler<>))
                .First()
                .GetGenericArguments()
                .First();

            if (!dictionary.ContainsKey(message))
            {
                dictionary.Add(message, new List<Type>());
            }

            dictionary[message].Add(type);
        }

        private static List<Type> GetMessages(Assembly messagesAssembly)
        {
            Type[] types = messagesAssembly.GetExportedTypes();

            var list = types.Where(x => x.GetInterfaces().Contains(typeof (IMessage)))
                .ToList();           

            return list;
        }
    }
}