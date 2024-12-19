using UnityEngine;
namespace Emptybraces.Timeline
{
	public class SimpleNotifyTestQuake : MonoBehaviour, ISimpleNotifyReceiver
	{
        [SerializeField] float _power = 10f;
		public void OnNotify()
		{
			if (!Application.isPlaying)
				return;
			foreach (var i in GetComponentsInChildren<Rigidbody>())
			{
				if (i.IsSleeping())
				{
					i.AddForce(Vector3.up * _power, ForceMode.Impulse);
					i.AddTorque(Random.onUnitSphere, ForceMode.Impulse);
				}
			}
		}
	}
}
