namespace Footage.Application.Messages
{
    using System;
    using Footage.Application.ViewModel.Base;
    using Footage.Model;
    using GalaSoft.MvvmLight.Messaging;

    public static class MessengerHelper
    {
        /// <summary>
        /// Sends an <typeparamref name="TEntityMessage"/> marked with entity's <see cref="Entity.Id"/>.
        /// Only receivers registered with the same <see cref="Entity.Id"/> will receive the message.
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TEntityMessage"></typeparam>
        public static void Send<TEntityMessage>(TEntityMessage message) where TEntityMessage : EntityMessageBase
        {
            Messenger.Default.Send(message, message.EntityId);
        }

        /// <summary>
        /// Registers to <typeparamref name="TEntityMessage"/> messages marked with the same <see cref="Entity.Id"/>
        /// as the provided <paramref name="receiver"/>. This means the receiving entity will only receive messages
        /// about itself.
        /// </summary>
        public static void Register<TEntityMessage>(IEntityViewModel receiver, Action<TEntityMessage> action) where TEntityMessage : EntityMessageBase
        {
            Messenger.Default.Register(receiver, receiver.Id, action);
        }
    }
}