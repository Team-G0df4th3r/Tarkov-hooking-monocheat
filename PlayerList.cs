using EFT;
using EFT.InventoryLogic;
using MonoSecurity.Drawing;
using MonoSecurity.ESP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MonoSecurity
{
    public class PlayerList:MonoBehaviour
    {

        private static float Height = 0f;
        private static int count = 0;
        
        public void OnGUI()
        {
            if (GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && Main.LocalPlayer.PointOfView == EPointOfView.FirstPerson && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                if (Settings.DrawPlayerList)
                {
                    float num = 0f;
                    
                    Render.DrawBackground(new Rect((float)Screen.width - 270f, 50f, 250f, 100f + Height - 85f), new Color(0f, 0f, 0f, 0.8f)); //a 0.5f

                    foreach (GamePlayer gamePlayer in Main.GamePlayers)
                    {
                        if(!gamePlayer.IsAI && GameUtils.IsPlayerValid(gamePlayer.Player) && GameUtils.IsPlayerAlive(gamePlayer.Player) && !GameUtils.IsInYourGroup(gamePlayer.Player) && gamePlayer.Player != Main.LocalPlayer)
                        {
                            String PlayerType;
                            if (gamePlayer.Player.Profile.Info.MemberCategory == EMemberCategory.UniqueId || gamePlayer.Player.Profile.Info.MemberCategory == EMemberCategory.Default)
                            {
                                PlayerType = "";
                            }
                            else
                            {
                                PlayerType = $"[{gamePlayer.Player.Profile.Info.MemberCategory}]";
                            }

                            String PlayerInfo = $"{gamePlayer.Player.Profile.Info.Nickname} [{gamePlayer.Player.HealthController.GetBodyPartHealth(EBodyPart.Common, true).Current}]HP [{gamePlayer.Player.Profile.Info.Level}] [{PlayerValue(gamePlayer.Player)/1000}K] {PlayerType}";

                            Render.DrawString(new Vector2((float)Screen.width - 260f, 53.04348f + num), PlayerInfo, Color.white, false, 9);
                            num += 17f;
                            Height = num;
                        }
                    }
                }
            }     
        }

        private static int PlayerValue(Player Player)
        {
            int credit = 0;
            foreach (Item item in Player.Profile.Inventory.Equipment.GetAllItems())
            {
                if (item.Template._parent != "5448bf274bdc2dfc2f8b456a")
                    credit += item.Template.CreditsPrice;
            }
            return credit;
        }
    }


}
