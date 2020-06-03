using UnityEngine;
namespace MonoSecurity
{
    public class Loader
    {
        private static GameObject HookObject;

        /*
        public void Load()
        {
            this.HookObject = new GameObject();
            this.HookObject.AddComponent<Main>();
            DontDestroyOnLoad(this.HookObject);
        }
        */
        public static void init()
        {

            HookObject = new GameObject();
            HookObject.AddComponent<Main>();
            GameObject.DontDestroyOnLoad(HookObject);

            //new Main().Load();

            /*
            GameObject gameObject = GameObject.Find("Application(Main Client)");

            if (gameObject == null)
            {
                gameObject = new GameObject("Application(Main Client)");

                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
            if (gameObject.GetComponent<Main>() != null)
            {
                UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<Main>());
            }
            gameObject.AddComponent<Main>();
            */
        }
    }
}
