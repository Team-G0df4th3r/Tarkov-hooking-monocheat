using UnityEngine.SceneManagement;

namespace MonoSecurity
{
    internal class GameScene
    {

        public static Scene CurrentGameScene;
        private static string GetSceneName()
        {
            return GameScene.CurrentGameScene.name;
        }

        public static bool IsLoaded()
        {
            return GameScene.CurrentGameScene.isLoaded;
        }

        public static bool InMatch()
        {
            return GameScene.GetSceneName() != "EnvironmentUIScene" && GameScene.GetSceneName() != "MenuUIScene" && GameScene.GetSceneName() != "CommonUIScene" && GameScene.GetSceneName() != "MainScene" && GameScene.GetSceneName() != "";
        }

        public static void GetScene()
        {
            GameScene.CurrentGameScene = SceneManager.GetActiveScene();
        }



    }
}
