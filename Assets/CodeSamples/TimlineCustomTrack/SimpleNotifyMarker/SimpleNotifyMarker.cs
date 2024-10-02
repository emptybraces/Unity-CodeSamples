using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace Emptybraces.Timeline
{
    [Serializable, DisplayName("EmptyBraces/SimpleNotifyMarker")]
    public class SimpleNotifyMarker : Marker, INotification, INotificationOptionProvider
    {
        [Header("System")]
        public bool EmitOnce;
        public bool Retroactive;
        public bool ShowLineOverlay = true;
        public Color ShowLineOverlayColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        [Min(0)] public float ShowLineOverlayWidth = 6;
        public int ShowLineOverlayWidthOrder;
        [TextArea(2, 5)] public string Description;

        [Header("Target")]
        public ExposedMonobehaviour[] Receivers;
        [Serializable] public class ExposedReferenceHolder<T> where T : UnityEngine.Object { public ExposedReference<T> ExposedReference; }
        [Serializable] public sealed class ExposedMonobehaviour : ExposedReferenceHolder<MonoBehaviour> { }
        PropertyName INotification.id
        {
            get
            {
                return new PropertyName("EmptyBraces/SimpleNotifyMarker");
            }
        }

        NotificationFlags INotificationOptionProvider.flags
        {
            get
            {
                return (Retroactive ? NotificationFlags.Retroactive : default(NotificationFlags)) |
                    (EmitOnce ? NotificationFlags.TriggerOnce : default(NotificationFlags));
            }
        }
    }
}
