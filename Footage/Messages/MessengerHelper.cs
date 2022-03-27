namespace Footage.Messages
{
    using GalaSoft.MvvmLight.Messaging;

    public class MessengerHelper
    {
        public static void Send<TEntityMessage>(TEntityMessage message) where TEntityMessage : EntityMessageBase
        {
            Messenger.Default.Send(message, message.Id);
        }  
    }
}