using EFT.Interactive;
using EFT.InventoryLogic;
using MonoSecurity.Drawing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonoSecurity.ESP
{
    public class LootableContainerESP : MonoBehaviour
    {
        private static readonly float CacheLootContainerInterval = 120f;

        private float _nextLootContainerCacheTime;

        private List<GameLootContainer> _gameLootContainers;

        private static Color LootableContainerColour = new Color(1f, 0.2f, 0.09f);

        private static Color ListColour = Color.white;

        private static Color TitleColour = new Color(254, 161, 0);


        private bool IsSpecialLootItem(Item Item)
        {
            if (!(Item == null) && Item != null)
            {
                string item = Item.Name.Localized().ToLower();
                string item2 = Item.ShortName.Localized();
                return ItemESP.SpecialLootItems.Contains(item2) || item.Contains(Settings.ItemLookup);
            }
            return false;
        }

        public void Start()
        {
            _gameLootContainers = new List<GameLootContainer>();
        }

        public void FixedUpdate()
        {

            if (Settings.DrawLootableContainers && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                if (Time.time >= _nextLootContainerCacheTime && Main.GameWorld != null && Main.GameWorld.LootItems != null)
                {
                    _gameLootContainers.Clear();
                    foreach (LootableContainer lootableContainer in FindObjectsOfType<LootableContainer>()) //foreach (LootableContainer lootableContainer in FindObjectsOfType<LootableContainer>())
                    {
                        if (GameUtils.IsLootableContainerValid(lootableContainer) && Vector3.Distance(Main.MainCamera.transform.position, lootableContainer.transform.position) <= Settings.DrawLootableContainersDistance)
                        {
                            _gameLootContainers.Add(new GameLootContainer(lootableContainer));
                        }
                    }
                    _nextLootContainerCacheTime = Time.time + CacheLootContainerInterval;
                }
                foreach (GameLootContainer gameLootContainer in _gameLootContainers)
                {
                    gameLootContainer.RecalculateDynamics();
                }
            }


        }

        public void OnGUI()
        {
            if (Settings.DrawLootableContainers && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                int num = -20;
                int ItemValue = -1;
                string container = default;
                string greaterthanten = "";
                foreach (GameLootContainer gameLootContainer in _gameLootContainers)
                {
                    if (GameUtils.IsLootableContainerValid(gameLootContainer.LootableContainer) && gameLootContainer.IsOnScreen && gameLootContainer.Distance <= Settings.DrawLootableContainersDistance)
                    {
                        if (gameLootContainer.LootableContainer.ItemOwner != null && gameLootContainer.LootableContainer.ItemOwner.RootItem != null && gameLootContainer.LootableContainer.ItemOwner.RootItem.GetAllItems(false).Count() > 1)
                        {
                            if (Settings.DrawContainersContent && gameLootContainer.Distance <= Settings.DrawContainersListDistance)
                            {
                                Item rootItem = gameLootContainer.LootableContainer.ItemOwner.RootItem;
                                string label = rootItem.Name.Localized();
                                foreach (Item item in rootItem.GetAllItems(false))
                                {
                                    ItemValue = item.Template.CreditsPrice > 1000 ? item.Template.CreditsPrice / 1000 : item.Template.CreditsPrice;
                                    greaterthanten = item.Template.CreditsPrice > 1000 ? "K" : "";

                                    if (rootItem.GetAllItems(false).First() == item)
                                    {
                                        label = item.ShortName.Localized() + " [" + gameLootContainer.FormattedDistance + "]";
                                        LootableContainerColour = new Color(1f, 0.2f, 0.09f);
                                    }
                                    else
                                    {
                                        label = $"{item.ShortName.Localized()} [{ItemValue}{greaterthanten}]";
                                        LootableContainerColour = ListColour;
                                    }
                                    Render.DrawString(new Vector2(gameLootContainer.ScreenPosition.x, gameLootContainer.ScreenPosition.y - num), label, LootableContainerColour, true);
                                    num -= 20;
                                }

                                //Render.DrawString(new Vector2(gameLootContainer.ScreenPosition.x, gameLootContainer.ScreenPosition.y - (float)num), label, LootableContainerESP.LootableContainerColor, true);
                            }
                            if (!Settings.DrawContainersContent)
                            {
                                int creditsPrice = default;
                                Item rootItem = gameLootContainer.LootableContainer.ItemOwner.RootItem;
                                string label = rootItem.Name.Localized();
                                foreach (Item item in rootItem.GetAllItems())
                                {
                                    if (IsSpecialLootItem(item))
                                    {
                                        creditsPrice = ContainerValue(rootItem);
                                        container = $"{label} [{gameLootContainer.FormattedDistance}] [{creditsPrice}K]";
                                    }

                                }

                                Render.DrawString(new Vector2(gameLootContainer.ScreenPosition.x, gameLootContainer.ScreenPosition.y - num), container, TitleColour, true, 9);
                            }
                        }
                    }
                }
            }

        }

        private int ContainerValue(Item rootItem)
        {
            int credit = 0;
            foreach (Item item in rootItem.GetAllItems())
            {
                credit += item.Template.CreditsPrice;
            }
            return credit;
        }

    }
}
