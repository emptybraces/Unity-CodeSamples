﻿using UnityEngine;
using UnityEngine.Playables;
namespace Emptybraces.Timeline
{
    public class SimpleNotifyReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification is not SimpleNotifyMarker marker)
                return;
            var resolver = origin.GetGraph().GetResolver();
            foreach (var receiver in marker.Receivers)
            {
                if (receiver.ExposedReference.Resolve(resolver) is ISimpleNotifyReceiver notify)
                {
                    notify.OnNotify();
                }
            }
        }
    }
}