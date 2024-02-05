using UnityEditor;
using System.Linq;
using System;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine.XR.OpenXR;

namespace EmptyBraces.Editor
{
	public static class CustomTopBarMenu
	{
		const string menuName = "Custom";
		[MenuItem(menuName + "/Define Symbol/Add VR", false)] static void _AddDefineSymbolVR() => _EditDefineSymbol(false, "VR");
		[MenuItem(menuName + "/Define Symbol/Add VR", true)] static bool _AddDefineSymbolVRValdiate() => _EditDefineSymbolValidate(false, "VR");
		[MenuItem(menuName + "/Define Symbol/Remove VR", false)] static void _RemoveDefineSymbolVR() => _EditDefineSymbol(true, "VR");
		[MenuItem(menuName + "/Define Symbol/Remove VR", true)] static bool _RemoveDefineSymbolVRValidate() => _EditDefineSymbolValidate(true, "VR");
		static bool _EditDefineSymbolValidate(bool isRemove, string symbol)
		{
			// var defines = EditorUserBuildSettings.activeScriptCompilationDefines;
			var editor_target = EditorUserBuildSettings.selectedBuildTargetGroup;
			var build_target = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(editor_target);
			var extra_defines = PlayerSettings.GetScriptingDefineSymbols(build_target).Split(";");
			return isRemove ? extra_defines.Contains(symbol) : !extra_defines.Contains(symbol);
		}
		static void _EditDefineSymbol(bool isRemove, string symbol)
		{
			var editor_target = EditorUserBuildSettings.selectedBuildTargetGroup;
			var build_target = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(editor_target);
			var extra_defines = PlayerSettings.GetScriptingDefineSymbols(build_target).Split(";").ToList();
			if (isRemove)
			{
				if (!extra_defines.Remove(symbol))
					return;
			}
			else
			{
				if (extra_defines.Contains(symbol))
					return;
				extra_defines.Add(symbol);
			}
			PlayerSettings.SetScriptingDefineSymbols(build_target, extra_defines.ToArray());
			var xrGeneralSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(editor_target);
			xrGeneralSettings.InitManagerOnStart = !isRemove;
			var buildTargetSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(editor_target);
			var pluginsSettings = buildTargetSettings.AssignedSettings;
			if (isRemove)
			{
				XRPackageMetadataStore.RemoveLoader(pluginsSettings, typeof(OpenXRLoader).FullName, editor_target);
			}
			else
			{
				XRPackageMetadataStore.AssignLoader(pluginsSettings, typeof(OpenXRLoader).FullName, editor_target);
			}
			EditorUtility.SetDirty(xrGeneralSettings);
			AssetDatabase.SaveAssets();
		}
	}
}