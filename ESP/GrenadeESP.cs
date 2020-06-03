using MonoSecurity.Drawing;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MonoSecurity.ESP
{
    internal class GrenadeESP : MonoBehaviour
    {
        private static readonly float CacheGrenadeInterval = 1f;

        private float _nextgrenadeCacheTime;

        private List<Throwable> _gameGrenades;

        private Vector3 screenPosition;

        private bool IsOnScreen;

        public float Distance;

        private static readonly Color labelcolour = Color.red;

        public string FormattedDistance
        {
            get
            {
                return string.Format("{0}m", Math.Round(Distance));
            }
        }

        //in Main.GameWorld.
        //Throwable throwable in FindObjectsOfType<Throwable>()
        public void Start()
        {
            _gameGrenades = new List<Throwable>();
        }

        public void FixedUpdate()
        {

            if (Settings.DrawGrenades && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                if (Time.time >= _nextgrenadeCacheTime && Main.GameWorld != null && Main.GameWorld.Grenades != null)
                {
                    _gameGrenades.Clear();
                    foreach (Throwable throwable in Main.GameWorld.Grenades.GetValuesEnumerator()) //
                    {
                        if (throwable != null && Vector3.Distance(Main.MainCamera.transform.position, throwable.transform.position) <= Settings.DrawGrenadeDistance)
                        {
                            _gameGrenades.Add(throwable);
                        }
                    }
                    _nextgrenadeCacheTime = Time.time + CacheGrenadeInterval;
                }
                foreach (Throwable throwable in _gameGrenades)
                {
                    if (throwable != null)
                    {
                        screenPosition = GameUtils.WorldPointToScreenPoint(throwable.transform.position);
                        Distance = Vector3.Distance(Main.MainCamera.transform.position, throwable.transform.position);
                        IsOnScreen = GameUtils.IsScreenPointVisible(screenPosition);
                    }
                }
            }
        }

        public void OnGUI()
        {
            if (Settings.DrawGrenades && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                foreach (Throwable throwable in _gameGrenades)
                {
                    if (throwable != null && Distance <= Settings.DrawGrenadeDistance && IsOnScreen)
                    {
                        string label = $"{GrenadeName(throwable)}  [{FormattedDistance}]";
                        Render.DrawString(new Vector2(screenPosition.x, screenPosition.y - 20f), label, labelcolour, true);
                        Render.DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(screenPosition.x, screenPosition.y), 0.8f, labelcolour);
                        Render.DrawBox(screenPosition.x, screenPosition.y, 5f, 5f, 0.8f, labelcolour);
                    }
                }
            }

        }

        private string GrenadeName(Throwable throwable)
        {
            if (throwable.name.ToLower().Contains("f1"))
            {
                return "F-1";
            }
            else if (throwable.name.ToLower().Contains("m67"))
            {
                return "M67";
            }
            else if (throwable.name.ToLower().Contains("rgd"))
            {
                return "RGD";
            }
            else if (throwable.name.ToLower().Contains("vog"))
            {
                return "VOG";
            }
            else if (throwable.name.ToLower().Contains("rdg"))
            {
                return "Smoke";
            }
            else if (throwable.name.ToLower().Contains("zarya"))
            {
                return "Flash";
            }
            else
            {
                return "Grenade";
            }
        }

    }
}
