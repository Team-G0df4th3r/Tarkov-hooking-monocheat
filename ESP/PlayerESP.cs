using EFT;
using EFT.InventoryLogic;
using MonoSecurity.Drawing;
using System;
using System.Linq;
using UnityEngine;
using static MonoSecurity.GamePlayer;

namespace MonoSecurity.ESP
{
    public class PlayerESP : MonoBehaviour
    {
        public static Color _playerColor = new Color(0.1254f, 0.7294f, 0.6784f); //new Color(32, 186, 173);

        public static Color _deadPlayerColor = Color.gray;

        public static Color _botColor = Color.white;

        public static Color _scavGuardColor = new Color(0.5294f, 0.8078f, 0.9215f);// new Color(135, 206, 235);

        public static Color _bossColor = new Color(1f, 1f, 0.4f); //new Color(255, 255, 102);

        public static Color _raiderColor = new Color(0.5294f, 0.8078f, 0.9215f);

        public static Color _friendColor = new Color(0.5411f, 0.1686f, 0.8862f);// new Color(138, 43, 226);


        public void OnGUI()
        {
            if (Settings.DrawPlayers && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                foreach (GamePlayer gamePlayer in Main.GamePlayers) //gamePlayer.Player.HealthController.GetBodyPartHealth(EBodyPart.Common, false).Current
                {
                    if (gamePlayer.IsOnScreen && gamePlayer.Distance <= Settings.DrawPlayersDistance && gamePlayer.Player != Main.LocalPlayer)
                    {
                        float boxHeight = Math.Abs(gamePlayer.ScreenPosition.y - gamePlayer.HeadScreenPosition.y);
                        string PlayerInfo = "";
                        string WeaponName = "";
                        int AmmoCount = -1;
                        int MagAmmoCount = -1;
                        string WeaponInfo = default;
                        string BulletName = "Empty";
                        int creditsPrice = default;
                        string PriceOfPlayer = default;
                        PlayerType playerType = default;
                        Color InitColor = default;

                        string customName = PlayerESP.PlayerInfo(gamePlayer, ref playerType, ref InitColor);

                        Color color = GameUtils.IsPlayerAlive(gamePlayer.Player) ? HealthColor(gamePlayer.Player.HealthController.GetBodyPartHealth(EBodyPart.Common, false).Current, InitColor) : _deadPlayerColor;

                        //Color color = HealthColor(gamePlayer.Player.HealthController.GetBodyPartHealth(EBodyPart.Common, false).Current, playerColor);

                        if (Settings.DrawPlayerBox && GameUtils.IsPlayerAlive(gamePlayer.Player) && gamePlayer.Distance <= Settings.DrawSkeletonDistance)
                        {
                            Render.CornerBox(new Vector2(gamePlayer.HeadScreenPosition.x, gamePlayer.HeadScreenPosition.y - 10f), boxHeight / 2.5f, boxHeight + 10f, 3f, color);
                        }
                        if (Settings.DrawPlayerLine && GameUtils.IsPlayerAlive(gamePlayer.Player) && !gamePlayer.IsAI)
                        {
                            Render.DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(gamePlayer.ScreenPosition.x, gamePlayer.ScreenPosition.y), 1f, color);//Aimbot.IsVisible(GameUtils.GetBonePosByID(gamePlayer.Player, 133)) ? Color.green : Color.red

                        }
                        if (Settings.DrawPlayerSkeleton && GameUtils.IsPlayerAlive(gamePlayer.Player) && gamePlayer.Player.HealthController.IsAlive && gamePlayer.Distance < Settings.DrawSkeletonDistance)
                        {
                            if (Settings.DrawScavSkeleton && playerType == PlayerType.Scav)
                            {
                                DrawSkeleton(gamePlayer, 1f, color);
                                if (Settings.DrawPlayerHeadLine && gamePlayer.Fov <= Settings.AimbotFOV)
                                    DrawHeadLine(gamePlayer);

                            }
                            else if (playerType != PlayerType.Scav)
                            {
                                DrawSkeleton(gamePlayer, 1f, color);
                                if (Settings.DrawPlayerHeadLine && gamePlayer.Fov <= Settings.AimbotFOV)
                                    DrawHeadLine(gamePlayer);
                            }

                        }

                        if (Settings.DrawPlayerName)
                        {
                            creditsPrice = PlayerValue(gamePlayer.Player);

                            if (gamePlayer.Player.HandsController != null && gamePlayer.Player.HandsController.Item != null)
                            {
                                WeaponName = gamePlayer.Player.HandsController.Item.ShortName.Localized();
                                if (gamePlayer.Player.HandsController.Item.GetItemComponent<KnifeComponent>() == null)
                                {
                                    if (gamePlayer.Player.HandsController.Item is Weapon && gamePlayer.Player.HandsController.Item.GetCurrentMagazine() != null && gamePlayer.Player.Weapon != null)
                                    {
                                        //.Weapon.ShortName.Localized();
                                        AmmoCount = gamePlayer.Player.HandsController.Item.GetCurrentMagazine().Cartridges.Count + gamePlayer.Player.Weapon.ChamberAmmoCount;
                                        MagAmmoCount = gamePlayer.Player.HandsController.Item.GetCurrentMagazine().Cartridges.MaxCount;
                                        if (gamePlayer.Player.HandsController.Item.GetCurrentMagazine().Cartridges.Items.FirstOrDefault() != null)
                                        {
                                            BulletName = gamePlayer.Player.HandsController.Item.GetCurrentMagazine().Cartridges.Items.FirstOrDefault().ShortName.Localized();
                                        }
                                    }
                                }
                            }

                            if (GameUtils.IsPlayerAlive(gamePlayer.Player))
                            {
                                if (playerType == PlayerType.Player || playerType == PlayerType.Friend)
                                {
                                    PlayerInfo = $"{gamePlayer.Player.Profile.Info.Nickname} [{gamePlayer.FormattedDistance}]";
                                    PriceOfPlayer = $" ({gamePlayer.Player.Profile.Info.Level}) [{creditsPrice / 1000}K]";
                                }
                                else
                                {
                                    PlayerInfo = $"{customName} [{gamePlayer.FormattedDistance}]";
                                    PriceOfPlayer = $" ({gamePlayer.Player.Profile.Info.Level}) [{creditsPrice / 1000}K]";
                                }
                            }

                            WeaponInfo = $"{WeaponName} ({AmmoCount}/{MagAmmoCount})";

                            if (!GameUtils.IsPlayerAlive(gamePlayer.Player) && playerType == PlayerType.Player)
                            {
                                PlayerInfo = $"{gamePlayer.Player.Profile.Info.Nickname} [{gamePlayer.FormattedDistance}] [{creditsPrice / 1000}K]";
                            }

                            if (!gamePlayer.IsAI && GameUtils.IsPlayerAlive(gamePlayer.Player) && Settings.HideNames)
                            {
                                PlayerInfo = $"Nigger [{gamePlayer.FormattedDistance}]";
                                PriceOfPlayer = $"({gamePlayer.Player.Profile.Info.Level}) [{creditsPrice / 1000}K]";
                            }

                            Render.DrawString(new Vector2(gamePlayer.HeadScreenPosition.x, gamePlayer.HeadScreenPosition.y - 29f), PlayerInfo, color, true, 8, FontStyle.Bold);
                            if (PriceOfPlayer != null)
                            {
                                Render.DrawString(new Vector2(gamePlayer.HeadScreenPosition.x, gamePlayer.HeadScreenPosition.y - 20f), PriceOfPlayer, color, true, 8, FontStyle.Bold);
                            }
                            if (WeaponInfo != default)
                            {
                                if (gamePlayer.Fov <= 230f && gamePlayer.Distance <= Settings.DrawSkeletonDistance && GameUtils.IsPlayerAlive(gamePlayer.Player))
                                {
                                    Vector2 PlayerInfoWeaponV2 = GUI.skin.GetStyle(WeaponInfo).CalcSize(new GUIContent(WeaponInfo));
                                    Render.DrawString(new Vector2(gamePlayer.ScreenPosition.x, gamePlayer.ScreenPosition.y + 5f), WeaponInfo, color, true, 8, FontStyle.Bold);
                                    Render.DrawString(new Vector2(gamePlayer.ScreenPosition.x, gamePlayer.ScreenPosition.y + 15f), $"[{BulletName}]", color, true, 8, FontStyle.Bold);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void DrawSkeleton(GamePlayer gamePlayer, float thickness, Color color)
        {
            int radius;
            if ((int)gamePlayer.Distance > 0)
            {
                radius = 100 / (int)gamePlayer.Distance;
            }
            else
            {
                radius = 100;
            }

            Vector3 RightPalm = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.RightPalm.position);
            Vector3 LeftPalm = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.LeftPalm.position);
            Vector3 LeftShoulder = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.LeftShoulder.position);
            Vector3 RightShoulder = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.RightShoulder.position);
            Vector3 Neck = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.Neck.position);
            Vector3 Pelvis = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.Pelvis.position);
            Vector3 KickingFoot = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.KickingFoot.position);
            Vector3 Leftfoot = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 18));
            Vector3 LeftElbow = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 91));
            Vector3 RightElbow = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 112));
            Vector3 LeftKnee = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 17));
            Vector3 RightKnee = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 22));
            Vector3 head = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 133));

            Render.DrawLine(new Vector2(Neck.x, Screen.height - Neck.y), new Vector2(Pelvis.x, Screen.height - Pelvis.y), thickness, color);
            Render.DrawLine(new Vector2(LeftShoulder.x, Screen.height - LeftShoulder.y), new Vector2(LeftElbow.x, Screen.height - LeftElbow.y), thickness, color);
            Render.DrawLine(new Vector2(RightShoulder.x, Screen.height - RightShoulder.y), new Vector2(RightElbow.x, Screen.height - RightElbow.y), thickness, color);
            Render.DrawLine(new Vector2(LeftElbow.x, Screen.height - LeftElbow.y), new Vector2(LeftPalm.x, Screen.height - LeftPalm.y), thickness, color);
            Render.DrawLine(new Vector2(RightElbow.x, Screen.height - RightElbow.y), new Vector2(RightPalm.x, Screen.height - RightPalm.y), thickness, color);
            Render.DrawLine(new Vector2(RightShoulder.x, Screen.height - RightShoulder.y), new Vector2(LeftShoulder.x, Screen.height - LeftShoulder.y), thickness, color);
            Render.DrawLine(new Vector2(LeftKnee.x, Screen.height - LeftKnee.y), new Vector2(Pelvis.x, Screen.height - Pelvis.y), thickness, color);
            Render.DrawLine(new Vector2(RightKnee.x, Screen.height - RightKnee.y), new Vector2(Pelvis.x, Screen.height - Pelvis.y), thickness, color);
            Render.DrawLine(new Vector2(LeftKnee.x, Screen.height - LeftKnee.y), new Vector2(Leftfoot.x, Screen.height - Leftfoot.y), thickness, color);
            Render.DrawLine(new Vector2(RightKnee.x, Screen.height - RightKnee.y), new Vector2(KickingFoot.x, Screen.height - KickingFoot.y), thickness, color);
            Render.DrawLine(new Vector2(Neck.x, Screen.height - Neck.y), new Vector2(head.x, Screen.height - head.y), thickness, color);
            Circle.DrawCircleMain(new Vector2(head.x, Screen.height - head.y), radius, color, thickness, false, 6);
        }

        private static void DrawHeadLine(GamePlayer gamePlayer)
        {
            //Vector3 WeaponPos = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.WeaponRoot.position);
            Vector3 HeadPos = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 133));
            Vector3 UpAndForward = gamePlayer.Player.PlayerBones.WeaponRoot.up * 6f;
            Vector3 WeaponPosForward = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.WeaponRoot.position - UpAndForward);
            Render.DrawLine(new Vector2(WeaponPosForward.x, Screen.height - WeaponPosForward.y), new Vector2(HeadPos.x, Screen.height - HeadPos.y), 1.5f, Color.red);
        }

        private int Setfontsize(float distance)
        {
            switch (distance)
            {
                case float dis when dis <= 50:
                    return 13;
                case float dis when dis < 100 && dis > 50:
                    return 12;
                case float dis when dis > 100 && dis <= 200:
                    return 11;
                case float dis when dis > 200 && dis <= 300:
                    return 8;
                default:
                    return 8;
            }
        }

        private Color HealthColor(float currentHealth, Color playerColor)
        {
            switch (currentHealth)
            {
                case float htlh when htlh >= 400f:
                    return playerColor;
                case float htlh when htlh > 300 && htlh <= 400f:
                    return Color.yellow;
                case float htlh when htlh > 200f && htlh <= 300f:
                    return new Color(255f, 128f, 0f);
                case float htlh when htlh > 100f && htlh <= 200f:
                    return new Color(1f, 0.7f, 0.16f);
                case float htlh when htlh <= 100f:
                    return Color.red;
                default:
                    return playerColor;
            }
        }

        public static Color PlayerColor(ref PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Friend:
                    return _friendColor;
                case PlayerType.Player:
                    return _playerColor;
                case PlayerType.Scav:
                    return _botColor;
                case PlayerType.Raider:
                    return _raiderColor;
                case PlayerType.PlayerScav:
                    return _playerColor;
                case PlayerType.Boss:
                    return _bossColor;
                case PlayerType.ScavGuard:
                    return _scavGuardColor;
                default:
                    return _playerColor;
            }
        }

        public static string PlayerInfo(GamePlayer gamePlayer, ref PlayerType playerType, ref Color color)
        {
            if (GameUtils.IsInYourGroup(gamePlayer.Player))
            {
                playerType = PlayerType.Friend;
                color = _friendColor;
                return "";
            }
            else if (gamePlayer.Player.Profile.Info.RegistrationDate <= 0)
            {
                if (gamePlayer.Player.Profile.Info.Settings.Role.ToString().IndexOf("boss") != -1)
                {
                    playerType = PlayerType.Boss;
                    color = _bossColor;
                    return "Boss";
                }
                else if (gamePlayer.Player.Profile.Info.Settings.Role.ToString().IndexOf("follower") != -1)
                {
                    playerType = PlayerType.ScavGuard;
                    color = _scavGuardColor;
                    return "Scav Guard";
                }
                else if (gamePlayer.Player.Profile.Info.Settings.Role.ToString().ToLower().IndexOf("pmcbot") != -1)
                {
                    playerType = PlayerType.Raider;
                    color = _raiderColor;
                    return "Raider";
                }
                else
                {
                    playerType = PlayerType.Scav;
                    color = _botColor;
                    return "Kike";
                }
            }
            else if (gamePlayer.Player.Profile.Info.Side == EPlayerSide.Savage) //&& !())
            {
                playerType = PlayerType.PlayerScav;
                color = _playerColor;
                return "Pscav";
            }
            else
            {
                playerType = PlayerType.Player;
                color = _playerColor;
                return "";
            }
        }


        private static int PlayerValue(Player Player)
        {
            int credit = 0;
            foreach (Item item in Player.Profile.Inventory.Equipment.GetAllItems())
            {
                if (item.Template._parent != "5448bf274bdc2dfc2f8b456a")
                    credit += item.Template.CreditsPrice;
                /*
                if (!Main.LocalPlayer.Profile.Encyclopedia.ContainsKey(item.TemplateId))
                {
                    Main.LocalPlayer.Profile.Encyclopedia.Add(item.TemplateId, false);
                }
                */

            }
            return credit;
        }

        private int StashValue(GamePlayer gamePlayer)
        {
            int credit = 0;
            foreach (Item item in gamePlayer.Player.Profile.Inventory.Stash.GetAllItems())
            {
                credit += item.Template.CreditsPrice;
            }
            return credit;
        }




        public static bool LocalPlayerisVisible(Vector3 LineEnd)
        {
            int vis_mask = 1 << 12 | 1 << 16;
            RaycastHit raycastHit;
            return Physics.Linecast(LineEnd, Main.LocalPlayer.PlayerBones.Head.position, out raycastHit, vis_mask) && raycastHit.collider && raycastHit.collider.gameObject.transform.root.gameObject == Main.LocalPlayer.gameObject.transform.root.gameObject;
        }

        /* Credit to SweetHamCheeks on UC
            vector = scopeCamera.WorldToViewportPoint(position);
            vector.x = (vector.x - 0.5f) * 1.65f + 0.5f;
            vector.y = (vector.y - 0.5f) * 1.65f + 0.5f;
            Vector3 screen = scopeCamera.ViewportToScreenPoint(vector);
            screen.x += (float)(Camera.main.pixelWidth / 2 - scopeCamera.pixelWidth / 2);
            screen.y += (float)(Camera.main.pixelHeight / 2 - scopeCamera.pixelHeight / 2);
        */
    }
}

