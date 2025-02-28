using UnityEngine;
using UnityEngine.SceneManagement;
namespace Emptybraces.PassingDataNextSceneSample
{
	public class Sample_PassingDataNextSceneNext : MonoBehaviour
	{
		[SerializeField] StartSceneArgs _startSceneArgs;
		[SerializeField] TMPro.TMP_Text _text;
		static StartSceneArgs s_startSceneArgs;

		[System.Serializable]
		public class StartSceneArgs
		{
			public int TestIdx;
			public string TestStr;
			public override string ToString()
			{
				return $"{TestIdx}, {TestStr}";
			}
		}

		void Awake()
		{
			if (s_startSceneArgs == null)
				s_startSceneArgs = _startSceneArgs;
			_text.text = s_startSceneArgs.ToString();
		}

		public static void LoadScene(StartSceneArgs args)
		{
			s_startSceneArgs = args;
			SceneManager.LoadScene("SampleScene_PassingDataNextScene.02_next");
		}

		public void OnClick()
		{
			Sample_PassingDataNextSceneStart.LoadScene();
		}
#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void _DomainReset()
		{
			s_startSceneArgs = null;
		}
#endif
	}
}
