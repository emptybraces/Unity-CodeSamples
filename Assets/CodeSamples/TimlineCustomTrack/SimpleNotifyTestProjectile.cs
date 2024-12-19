using UnityEngine;
namespace Emptybraces.Timeline
{
    public class SimpleNotifyTestProjectile : MonoBehaviour, ISimpleNotifyReceiver
    {
        [SerializeField] float _projectilePower = 5f;
        [SerializeField] Rigidbody _prefab;
        [SerializeField] Transform _parent;
        void Awake()
        {
            _prefab.gameObject.SetActive(false);
        }
        public void OnNotify()
        {
            if (!Application.isPlaying)
                return;
            var rb = Instantiate(_prefab, _parent);
            // Destroy(rb.gameObject, 3);
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
