using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Emptybraces.Timeline.Editor
{

	[CustomEditor(typeof(SimpleNotifyMarker), true)]
	[CanEditMultipleObjects]
	public class SimpleNotifyMarkerInspector : UnityEditor.Editor/* , IInspectorChangeHandler */
	{
		SimpleNotifyMarker _marker;
		GameObject _boundGameObject;
		PlayableDirector _associatedDirector;
		static GUIStyle _errorLabelStyle;
		// UnityEditor.Editor _defaultMarkerInspector;


		void OnEnable()
		{
			// if (_defaultMarkerInspector == null)
			//     _defaultMarkerInspector = UnityEditor.Editor.CreateEditor(targets, System.Type.GetType("UnityEditor.Timeline.MarkerInspector, Unity.Timeline.Editor"));
			_marker = target as SimpleNotifyMarker;
			_associatedDirector = TimelineEditor.inspectedDirector;
			if (_errorLabelStyle == null)
			{
				_errorLabelStyle = new GUIStyle(EditorStyles.label)
				{
					normal = { textColor = Color.red },
					fontStyle = FontStyle.Bold,
					wordWrap = true
				};
			}
		}

		// void OnDisable()
		// {
		//     var method = _defaultMarkerInspector.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		//     method?.Invoke(_defaultMarkerInspector, null);
		//     DestroyImmediate(_defaultMarkerInspector);
		// }
		
		public override void OnInspectorGUI()
		{
			using (var changeScope = new EditorGUI.ChangeCheckScope())
			{
				var property = serializedObject.GetIterator();
				var expanded = true;
				while (property.NextVisible(expanded))
				{
					expanded = false;
					if (property.propertyPath == "m_Script")
						continue;
					EditorGUILayout.PropertyField(property, true);
				}
				if (changeScope.changed)
				{
					serializedObject.ApplyModifiedProperties();
					TimelineEditor.Refresh(RefreshReason.ContentsModified | RefreshReason.WindowNeedsRedraw);
				}

			}
			_boundGameObject = GetBoundGameObject(_marker.parent, _associatedDirector);
			if (_boundGameObject != null && _boundGameObject.GetComponent<INotificationReceiver>() == null)
			{
				GUILayout.Label("Warning: Timeline object has not INotificationReceiver", _errorLabelStyle);
			}
			if (_boundGameObject != null && _associatedDirector != null)
			{
				var exposed_receivers = serializedObject.FindProperty(nameof(SimpleNotifyMarker.ExposedReceivers));
				for (int i = 0; i < exposed_receivers.arraySize; i++)
				{
					var element = exposed_receivers.GetArrayElementAtIndex(i);
					var exposed_name_prop = element.FindPropertyRelative("exposedName");
					var prop_name = new PropertyName(exposed_name_prop.stringValue);
					var resolved_object = _associatedDirector.GetReferenceValue(prop_name, out bool is_valid);
					if (is_valid && resolved_object is GameObject go)
					{
						if (go.GetComponent<ISimpleNotifyReceiver>() == null)
						{
							GUILayout.Label($"Warning: {go.name} has not ISimpleNotifyReceiver", _errorLabelStyle);
						}
					}
				}
			}
		}

		GameObject GetBoundGameObject(TrackAsset track, PlayableDirector associatedDirector)
		{
			if (associatedDirector == null || track == null) //if in asset mode, no bound object for you
				return null;

			var boundObj = GetSceneGameObject(associatedDirector, track);

			//if the signal is on the timeline marker track and user did not set a binding, assume it's bound to PlayableDirector
			if (boundObj == null && track.timelineAsset.markerTrack == track)
				boundObj = associatedDirector.gameObject;

			return boundObj;
		}
		GameObject GetSceneGameObject(PlayableDirector director, TrackAsset asset)
		{
			if (director == null || asset == null)
				return null;

			asset = GetSceneReferenceTrack(asset);

			var gameObject = director.GetGenericBinding(asset) as GameObject;
			var component = director.GetGenericBinding(asset) as Component;
			if (component != null)
				gameObject = component.gameObject;
			return gameObject;
		}
		TrackAsset GetSceneReferenceTrack(TrackAsset asset)
		{
			if (asset == null)
				return null;
			if (asset.isSubTrack)
				return GetSceneReferenceTrack(asset.parent as TrackAsset);
			return asset;
		}

		// public void OnPlayableAssetChangedInInspector()
		// {
		//     var method = _defaultMarkerInspector.GetType().GetMethod("OnPlayableAssetChangedInInspector", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		//     method?.Invoke(_defaultMarkerInspector, null);
		// }
	}
}
