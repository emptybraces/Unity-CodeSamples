using UnityEngine;
using UnityEngine.SceneManagement;
namespace Emptybraces.PassingDataNextSceneSample
{
	public class Sample_PassingDataNextSceneStart : MonoBehaviour
	{
		[SerializeField] TMPro.TMP_InputField _inputField;
		public void OnClick()
		{
			Sample_PassingDataNextSceneNext.LoadScene(new Sample_PassingDataNextSceneNext.StartSceneArgs { TestIdx = 1, TestStr = _inputField.text });
		}

		public static void LoadScene()
		{
			SceneManager.LoadScene("SampleScene_PassingDataNextScene.01_start");
		}
	}
}
