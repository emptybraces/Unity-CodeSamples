using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline
{
	[TrackColor(0.1f, 0.4f, 0.5f)]
	[TrackClipType(typeof(ObjectShakePlayableAsset))]
	[TrackBindingType(typeof(Transform))]
	public class ObjectShakeTrack : TrackAsset
	{
		// public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		// {
		// 	var trackBinding = director.GetGenericBinding(this) as Transform;
		// 	if (trackBinding == null)
		// 		return;
		// 	// driver.AddFromName<Transform>("m_LocalRotation.x");
		// 	// driver.AddFromName<Transform>("m_LocalRotation.y");
		// 	// driver.AddFromName<Transform>("m_LocalRotation.z");
		// 	// driver.AddFromName<Transform>("m_LocalRotation.x");
		// 	// driver.AddFromName<Transform>("m_LocalRotation.y");
		// 	// driver.AddFromName<Transform>("m_LocalRotation.z");
		// 	// driver.AddFromName<Transform>("m_LocalRotation.w");
		// 	base.GatherProperties(director, driver);
		// }
	}
}
