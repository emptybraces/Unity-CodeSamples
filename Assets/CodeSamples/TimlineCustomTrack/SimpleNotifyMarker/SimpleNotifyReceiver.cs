using UnityEngine;
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
            foreach (var receiver in marker.ExposedReceivers)
            {
                if (receiver.Resolve(resolver) is GameObject g)
                {
					foreach (var c in g.GetComponents<ISimpleNotifyReceiver>())
	                    c.OnNotify();
                }
            }
        }
    }
}
