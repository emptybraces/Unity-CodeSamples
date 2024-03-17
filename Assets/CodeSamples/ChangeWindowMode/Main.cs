using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Emptybraces.ChangeWindowMode
{
	public enum Mode { Windowed, Borderless, Fullscreen }
	public class Main : MonoBehaviour
	{
		[SerializeField] Button _buttonWindowed;
		[SerializeField] Button _buttonBorderless;
		[SerializeField] Button _buttonFullscreen;
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;        // x position of upper-left corner
			public int Top;         // y position of upper-left corner
			public int Right;       // x position of lower-right corner
			public int Bottom;      // y position of lower-right corner
		}
		// index for style and extended style flag management
		const int GWL_STYLE = -16, GWL_EXSTYLE = -20, WS_CAPTION = 0x00C00000;
		[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);
		[DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);
		[DllImport("user32.dll", EntryPoint = "SetWindowText", SetLastError = true)]
		public static extern bool SetWindowText(IntPtr hWnd, string lpString);
		[DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)] public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
		[DllImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT rect);
		[DllImport("user32.dll", EntryPoint = "GetDesktopWindow", SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
		public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int uFlags);
		IntPtr _hWnd;
		int _borderWidth;
		int _captionHeight;

#if UNITY_EDITOR
		void OnGUI()
		{
			GUI.Label(new Rect(100, 100, 1000, 100), "Process results cannot be checked on the editor.");
		}
#endif

		void Start()
		{
			_hWnd = FindWindow(IntPtr.Zero, Application.productName);
#if !UNITY_EDITOR
			SetWindowText(_hWnd, "Custom Window Title Name");
#endif
			GetDecorationSize();
			_buttonWindowed.onClick.AddListener(() => SetResolution(1280, 720, Mode.Windowed));
			_buttonBorderless.onClick.AddListener(() => SetResolution(1280, 720, Mode.Borderless));
			_buttonFullscreen.onClick.AddListener(() => SetResolution(1280, 720, Mode.Fullscreen));
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
				Application.Quit();
		}

		public void SetResolution(int w, int h, Mode mode)
		{
			if (Application.isEditor)
				return;
			StartCoroutine(__Local(w, h, mode));
			IEnumerator __Local(int w, int h, Mode mode)
			{
				switch (mode)
				{
					case Mode.Windowed:
						Screen.SetResolution(w, h, false);
						yield return null;
						// ウィンドウスタイルの変更
						var flags = (int)GetWindowLongPtr(_hWnd, GWL_STYLE);
						flags |= WS_CAPTION;
						SetWindowLongPtr(_hWnd, GWL_STYLE, flags);
						GetDecorationSize();
						// 位置の修正
						var position = GetCenteredPosition(w, h, mode);
						var windowWidth = w + _borderWidth * 2;
						var windowHeight = h + _captionHeight + _borderWidth * 2;
						SetWindowPos(_hWnd, -2, (int)position.x, (int)position.y, windowWidth, windowHeight, 0x0020);
						break;
					case Mode.Borderless:
						h -= _captionHeight;
						Screen.SetResolution(w, h, false);
						yield return null;
						flags = (int)GetWindowLongPtr(_hWnd, GWL_STYLE);
						flags &= ~WS_CAPTION;
						SetWindowLongPtr(_hWnd, GWL_STYLE, flags);
						// 位置の修正
						position = GetCenteredPosition(w, h, mode);
						SetWindowPos(_hWnd, -2, (int)position.x, (int)position.y, w, h, 0x0020);
						break;
					case Mode.Fullscreen:
						Screen.SetResolution(w, h, true);
						break;
				}
				yield break;
			}
			Vector2 GetCenteredPosition(int w, int h, Mode mode)
			{
				// desktop rect
				RECT desktopRect;
				if (!GetWindowRect(GetDesktopWindow(), out desktopRect))
					return Vector2.zero;
				int desktopWidth = desktopRect.Right - desktopRect.Left;
				int desktopHeight = desktopRect.Bottom - desktopRect.Top;

				// determine the centered position for the specified resolution
				int xPos, yPos;
				if (mode == Mode.Windowed)
				{
					xPos = (desktopWidth - (w + _borderWidth * 2)) / 2;
					yPos = (desktopHeight - (h + _borderWidth * 2 + _captionHeight)) / 2;
				}
				else
				{
					xPos = (desktopWidth - w) / 2;
					yPos = (desktopHeight - h) / 2;
				}
				return new Vector2(xPos, yPos);
			}
		}

		/// <summary>
		/// Gets the window's current decoration size. If the window is in borderless mode, the results will be 0.
		/// </summary>
		bool GetDecorationSize()
		{
			if (_borderWidth != 0 || _captionHeight != 0)
				return true;
			// window and client rects
			RECT windowRect, clientRect;
			if (!GetWindowRect(_hWnd, out windowRect)) return false;
			if (!GetClientRect(_hWnd, out clientRect)) return false;

			// calculate decoration size
			int decorationWidth = (windowRect.Right - windowRect.Left) - (clientRect.Right - clientRect.Left);
			int decorationHeight = (windowRect.Bottom - windowRect.Top) - (clientRect.Bottom - clientRect.Top);

			// some important assumptions are made here:
			// 1) the window's frame only has border on the left and right
			// 2) the window's frame only has an equal thickness border on the bottom
			_borderWidth = decorationWidth / 2;
			_captionHeight = decorationHeight - _borderWidth * 2;
			Debug.Log(_borderWidth + " " + _captionHeight);
			return true;
		}
	}
}