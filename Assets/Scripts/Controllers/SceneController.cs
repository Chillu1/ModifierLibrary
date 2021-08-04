using UnityEditor;

namespace CustomGameNamespace
{
    public class SceneController
    {
        public static void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        //TODO Load main menu
    }

}