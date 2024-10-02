using UnityEngine;
namespace Emptybraces.Timeline
{
    public class SimpleNotifyTest : MonoBehaviour, ISimpleNotifyReceiver
    {
        [SerializeField] float _projectilePower = 5f;
        Rigidbody _prefab;
        void Awake()
        {
            _prefab = transform.GetComponentInChildren<Rigidbody>();
            _prefab.gameObject.SetActive(false);
        }
        public void OnNotify()
        {
            if (!Application.isPlaying)
                return;
            var rb = Instantiate(_prefab, transform);
            Destroy(rb.gameObject, 3);
            ProjectileObject(rb);
        }
        void ProjectileObject(Rigidbody rb)
        {
            rb.gameObject.SetActive(true);
            var dir = rb.transform.forward;
            rb.AddForce(dir * _projectilePower, ForceMode.Impulse);
        }
    }
}
