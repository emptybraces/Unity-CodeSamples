using UnityEngine;
using UnityEngine.SceneManagement;
namespace Emptybraces.PassingDataNextScene
{
	public class Main : MonoBehaviour
	{
		[SerializeField] TMPro.TMP_InputField _inputField;
		public void OnClick()
		{
			SceneNext.LoadScene(new SceneNext.StartSceneArgs { TestIdx = 1, TestStr = _inputField.text });
		}

		public static void LoadScene()
		{
			SceneManager.LoadScene("Scene_PassingDataNextScene");
		}
	}
}
