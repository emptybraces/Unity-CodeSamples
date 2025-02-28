using UnityEngine;
namespace Emptybraces.Serializable
{
	[System.Serializable]
	public abstract class DrawableToggleValue
	{
		public bool Enabled;
	}
	[System.Serializable]
	public class ToggleBool : DrawableToggleValue
	{
		public bool Value;
		public ToggleBool(bool value)
		{
			this.Value = value;
		}
	}
	[System.Serializable]
	public class ToggleFloat : DrawableToggleValue
	{
		public float Value;
		public ToggleFloat(float value)
		{
			this.Value = value;
		}
	}
	[System.Serializable]
	public class ToggleInt : DrawableToggleValue
	{
		public int Value;
		public ToggleInt(int value)
		{
			this.Value = value;
		}
	}
	[System.Serializable]
	public class ToggleString : DrawableToggleValue
	{
		public string Value;
		public ToggleString(string value)
		{
			this.Value = value;
		}
	}
	[System.Serializable]
	public class ToggleColor : DrawableToggleValue
	{
		public Color Value;
		public ToggleColor(Color value)
		{
			this.Value = value;
		}
	}
	[System.Serializable]
	public class ToggleVector3 : DrawableToggleValue
	{
		public Vector3 Value;
		public ToggleVector3(Vector3 value)
		{
			this.Value = value;
		}
	}
}
