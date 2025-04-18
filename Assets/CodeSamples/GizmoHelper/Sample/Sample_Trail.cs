using UnityEngine;

namespace Emptybraces.GizmoHelperSample
{
	public class Sample_Trail : MonoBehaviour
	{
		[SerializeField] float _lifeTime = .5f;
		[SerializeField] int _drawIntervalFrame = 5;
		[SerializeField] float _speed = 1;
		[SerializeField] float _offset = 5;
		[SerializeField] Transform box, sphere;
		Vector3 _prevPos;

		void Update()
		{
			var p = Vector3.zero;
			p.x = Mathf.PerlinNoise1D(Time.time * _speed) * 2 - 1;
			p.y = Mathf.PerlinNoise1D(Time.time * _speed + 1) * 2 - 1;
			p.z = Mathf.PerlinNoise1D(Time.time * _speed + 2) * 2 - 1;
			box.position = p * _offset;

			p.x = Mathf.PerlinNoise1D(Time.time * _speed + 3) * 2 - 1;
			p.y = Mathf.PerlinNoise1D(Time.time * _speed + 4) * 2 - 1;
			p.z = Mathf.PerlinNoise1D(Time.time * _speed + 5) * 2 - 1;
			sphere.position = p * _offset;

			if (Time.frameCount % _drawIntervalFrame == 0 && Input.GetMouseButton(0))
			{
				var color = Color.HSVToRGB(Mathf.Repeat(Time.time, 1), 1f, .3f);
				color.a = 0.5f;
				GizmoHelper.DrawLine(box.position, sphere.position, color, _lifeTime);
				GizmoHelper.DrawCube(box.position, box.lossyScale, box.rotation, color, _lifeTime);
				GizmoHelper.DrawSphere(sphere.position, sphere.lossyScale.x / 2, color, _lifeTime);
				GizmoHelper.DrawDirection(box.position, (_prevPos - box.position).normalized, Color.red, _lifeTime);
			}
			_prevPos = box.position;
		}
	}
}
