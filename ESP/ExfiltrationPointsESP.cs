using EFT.Interactive;
using MonoSecurity.Drawing;
using System.Collections.Generic;
using UnityEngine;

namespace MonoSecurity.ESP
{
    public class ExfiltrationPointsESP : MonoBehaviour
    {
        private readonly List<GameExfiltrationPoint> _gameExfiltrationPoints = new List<GameExfiltrationPoint>();
        private static readonly float CacheExfiltrationPointInterval = 10f;
        private float _nextExfiltrationPointCacheTime;
        private static readonly Color ExfiltrationPointColour = Color.cyan;


        public void FixedUpdate()
        {
            if (Settings.DrawExfiltrationPoints && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                if (Time.time >= _nextExfiltrationPointCacheTime && Main.GameWorld.ExfiltrationController.ExfiltrationPoints != null)
                {
                    _gameExfiltrationPoints.Clear();
                    foreach (ExfiltrationPoint exfiltrationPoint in Main.GameWorld.ExfiltrationController.ExfiltrationPoints)
                    {
                        if (GameUtils.IsExfiltrationPointValid(exfiltrationPoint))
                        {
                            _gameExfiltrationPoints.Add(new GameExfiltrationPoint(exfiltrationPoint));
                        }
                    }
                    _nextExfiltrationPointCacheTime = Time.time + CacheExfiltrationPointInterval;
                }
                foreach (GameExfiltrationPoint gameExfiltrationPoint in _gameExfiltrationPoints)
                {
                    gameExfiltrationPoint.RecalculateDynamics();
                }
            }

        }

        private void OnGUI()
        {
            if (Settings.DrawExfiltrationPoints && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                foreach (GameExfiltrationPoint gameExfiltrationPoint in _gameExfiltrationPoints)
                {
                    if (GameUtils.IsExfiltrationPointValid(gameExfiltrationPoint.ExfiltrationPoint) && gameExfiltrationPoint.IsOnScreen)
                    {
                        string ExFillabel = gameExfiltrationPoint.ExfiltrationPoint.Settings.Name + " [" + gameExfiltrationPoint.FormattedDistance + "] " + ExfiltrationStatus(gameExfiltrationPoint.ExfiltrationPoint.Status);
                        Render.DrawString(new Vector2(gameExfiltrationPoint.ScreenPosition.x - 50f, gameExfiltrationPoint.ScreenPosition.y), ExFillabel, ExfiltrationPointColour, true, 7);
                    }
                }
            }
        }

        public static string ExfiltrationStatus(EExfiltrationStatus status)
        {
            switch (status)
            {
                case EExfiltrationStatus.AwaitsManualActivation:
                    return "Activate";
                case EExfiltrationStatus.Countdown:
                    return "Timer";
                case EExfiltrationStatus.NotPresent:
                    return "Closed";
                case EExfiltrationStatus.Pending:
                    return "Pending";
                case EExfiltrationStatus.RegularMode:
                    return "Open";
                case EExfiltrationStatus.UncompleteRequirements:
                    return "UncompReq.";
                default:
                    return "";
            }
        }
    }
}
