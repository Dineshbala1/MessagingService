using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagingServiceExtension
{
    public static class MessagingService
    {
        private static Dictionary<Tuple<string, Type, Type>, List<Tuple<WeakReference, Action<object, object>>>> callBackCollection;

        static MessagingService()
        {
            if (callBackCollection == null)
                callBackCollection = new Dictionary<Tuple<string, Type, Type>, List<Tuple<WeakReference, Action<object, object>>>>();
        }

        #region Public Methods

        /// <summary>
        /// Method to Subscribe for a named message from a valid publisher.
        /// </summary>
        /// <remarks>To be added.</remarks>
        public static void SubscribeToMessage<TMsgSender>(object subscriber, string message, Action<TMsgSender> callback, TMsgSender source = null) where TMsgSender : class
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException("subscriber");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            Action<object, object> actionCallback = delegate (object sender, object args)
            {
                TMsgSender tSender = (TMsgSender)((object)sender);
                if (source == null || tSender == source)
                {
                    callback((TMsgSender)((object)sender));
                }
            };
            SubscribeToMessage(subscriber, message, typeof(TMsgSender), null, actionCallback);
        }

        /// <summary>Unsubscribes a subscriber from the specified messages 
        /// that come from the specified publisher.</summary>
        /// <remarks></remarks>
        public static void UnsubscribeMessage<TMsgSender>(object subscriber, string message) where TMsgSender : class
        {
            UnsubscribeMessage(message, typeof(TMsgSender), null, subscriber);
        }

        /// <summary>Method to publish the message.</summary>
        /// <remarks>To be added.</remarks>
        public static void PublishMessage<TMsgSender>(TMsgSender sender, string message) where TMsgSender : class
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            PublishMessage(message, typeof(TMsgSender), sender, null);
        }

        /// <summary>
        /// Method to clear all the subscription with the message publisher.
        /// </summary>
        public static void ClearAllSubscribers()
        {
            callBackCollection.Clear();
        }

        #endregion

        #region PrivateMethods

        private static void UnsubscribeMessage(string message, Type senderType, Type argType, object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException("subscriber");
            }
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            var key = new Tuple<string, Type, Type>(message, senderType, argType);
            if (!callBackCollection.ContainsKey(key))
            {
                return;
            }
            callBackCollection[key].RemoveAll((Tuple<WeakReference, Action<object, object>> tuple) => !tuple.Item1.IsAlive || tuple.Item1.Target == subscriber);
            if (!callBackCollection[key].Any())
            {
                callBackCollection.Remove(key);
            }
        }

        private static void SubscribeToMessage(object subscriber, string message, Type senderType, Type argType, Action<object, object> callback)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            var key = new Tuple<string, Type, Type>(message, senderType, argType);
            var item = new Tuple<WeakReference, Action<object, object>>(new WeakReference(subscriber), callback);
            if (callBackCollection.ContainsKey(key))
            {
                callBackCollection[key].Add(item);
                return;
            }
            var value = new List<Tuple<WeakReference, Action<object, object>>>
            {
                item
            };
            callBackCollection[key] = value;
        }

        private static void PublishMessage(string message, Type msgSenderType, object sender, object args)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            var key = new Tuple<string, Type, Type>(message, msgSenderType, null);
            if (!callBackCollection.ContainsKey(key))
            {
                return;
            }
            var list = callBackCollection[key];
            if (list == null || !list.Any())
            {
                return;
            }
            foreach (Tuple<WeakReference, Action<object, object>> current in list.ToList())
            {
                if (current.Item1.IsAlive && list.Contains(current))
                {
                    current.Item2(sender, args);
                }
            }
        }

        #endregion
    }
}
