using Comfort.Common;
using EFT;
using EFT.UI;
using MonoSecurity.ESP;
using NLog;
using System.Collections.Generic;
using UnityEngine;

namespace MonoSecurity
{
    public class Main : MonoBehaviour
    {
        public static List<GamePlayer> GamePlayers = new List<GamePlayer>();

        public static Player LocalPlayer = null;

        public static GameWorld GameWorld;

        public static Camera MainCamera;

        public GameObject HookObject;

        private float _nextPlayerCacheTime;

        private float _nextCameraCacheTime;

        private static readonly float _cachePlayersInterval = 5f;

        private static readonly float _cacheCameraInterval = 7f;

        public static Font Consolas = Font.CreateDynamicFontFromOSFont("Consolas", 12);

        public void Awake()
        {
            //b1ghook.init();

            this.HookObject = new GameObject();
            //this.HookObject.AddComponent<Main>();
            this.HookObject.AddComponent<Menu>();
            this.HookObject.AddComponent<PlayerESP>();
            this.HookObject.AddComponent<PlayerList>();
            this.HookObject.AddComponent<ItemESP>();
            this.HookObject.AddComponent<LootableContainerESP>();
            this.HookObject.AddComponent<GrenadeESP>();
            this.HookObject.AddComponent<ExfiltrationPointsESP>();
            this.HookObject.AddComponent<Aimbot>();
            this.HookObject.AddComponent<Misc>();
            DontDestroyOnLoad(this.HookObject);
            GameScene.CurrentGameScene = default;

            //ByeBE.Init();
            //Aimbot.init();

        }

        //!MonoBehaviourSingleton<PreloaderUI>.Instance.IsBackgroundBlackActive

        public void FixedUpdate()
        {
            GameScene.GetScene();
            if (Time.time >= this._nextCameraCacheTime && !MonoBehaviourSingleton<PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                Main.GameWorld = Singleton<GameWorld>.Instance;
                Main.MainCamera = Camera.main;
                this._nextCameraCacheTime = Time.time + Main._cacheCameraInterval;
                if (MonoBehaviourSingleton<PreloaderUI>.Instance != null)
                {
                    MonoBehaviourSingleton<PreloaderUI>.Instance.SetSessionId("GAS THE KIKES");
                }
                if (LocalPlayer == null && Main.GameWorld != null && Main.GameWorld.RegisteredPlayers != null)
                {
                    foreach (Player player in Main.GameWorld.RegisteredPlayers) //foreach (Player player in FindObjectsOfType<Player>())
                    {
                        if (player.IsYourPlayer())
                        {
                            Main.LocalPlayer = player;
                            break;
                        }
                    }
                }
            }
            this.UpdatePlayers();
        }

        private void UpdatePlayers()
        {
            try
            {
                if (Settings.DrawPlayers && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive && Main.MainCamera != null)
                {
                    if (Time.time >= this._nextPlayerCacheTime && Main.GameWorld != null && Main.GameWorld.RegisteredPlayers != null)
                    {
                        Main.GamePlayers.Clear();
                        foreach (Player player in Main.GameWorld.RegisteredPlayers)
                        {
                            if (Vector3.Distance(Main.MainCamera.transform.position, player.Transform.position) <= Settings.DrawPlayersDistance)
                            {
                                Main.GamePlayers.Add(new GamePlayer(player));
                            }
                        }
                        this._nextPlayerCacheTime = Time.time + Main._cachePlayersInterval;
                    }
                    foreach (GamePlayer gamePlayer in Main.GamePlayers)
                    {
                        if (GameUtils.IsPlayerValid(gamePlayer.Player))
                            gamePlayer.RecalculateDynamics();
                    }
                }
            }
            catch
            { }
        }


    }
}




