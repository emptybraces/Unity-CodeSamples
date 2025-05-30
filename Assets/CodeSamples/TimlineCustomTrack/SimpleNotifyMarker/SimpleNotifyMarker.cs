﻿using System;
using System.Collections.Generic;
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
		[Min(0)] public float ShowLineOverlayWidth = 3;
		[TextArea(2, 5)] public string Description;

		[Header("Receivers")]
		public ExposedReference<GameObject>[] ExposedReceivers;
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

#if UNITY_EDITOR
		void OnValidate()
		{
			if (ExposedReceivers == null)
				return;
			List<PropertyName> hash = new();
			for (int i = 0; i < ExposedReceivers.Length; i++)
			{
				if (hash.Contains(ExposedReceivers[i].exposedName))
					ExposedReceivers[i].exposedName = new PropertyName();
				hash.Add(ExposedReceivers[i].exposedName);
			}
		}
#endif
	}
}
