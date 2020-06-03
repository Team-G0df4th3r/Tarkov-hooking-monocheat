using EFT.Interactive;
using EFT.InventoryLogic;
using JsonType;
using MonoSecurity.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonoSecurity.ESP
{
    public class ItemESP : MonoBehaviour
    {

        //public LootItemRarity LootItemRarity { get; private set; }

        public LootItemLabelState LootItemLabelState { get; private set; }

        private static readonly float CacheLootItemsInterval = 60f;

        private float _nextLootItemCacheTime;

        private static readonly Color SpecialColor = new Color(1f, 0.2f, 0.09f);

        private static readonly Color QuestColor = Color.yellow;

        private static readonly Color CommonColor = Color.white;

        private static readonly Color RareColor = new Color(0.38f, 0.43f, 1f);

        private static readonly Color SuperRareColor = new Color(1f, 0.29f, 0.36f);

        private static Color ListColour = new Color(1f, 0.2f, 0.09f);

        public static readonly List<GameLootItem> _gameLootItems = new List<GameLootItem>();

        public static List<string> SpecialLootItems;

        private Color pink = new Color(1f, 0.796078f, 1f);

        public void Start()
        {
            LootItemLabelState = LootItemLabelState.Special;

            SpecialLootItems = new List<string>
            {
                "LEDX",
                "Red",
                "Paracord",
                "Keycard",
                "Virtex",
                "Defibrillator",
                "0.2BTC",
                "Prokill",
                "Flash drive",
                "Violet",
                "Blue",
                "RB - PSP2",
                "RB - MP22",
                "RB - GN",
                "RR",
                "T - 7",
                "Green",
                "San.301",
                "Tetriz",
                "Dfuel",
                "Blue KC",
                "Black KC",
                "Violet keycard",
                "Blue keycard",
                "San.220",
                "KIBA",
                "Lion",
                "Clock",
                "Teapot",
                "Vase",
                "REAP-IR",
                "WCase",
                "SG-C10",
                "Fcond",
                "Checkpoint",
                "Reagent",
                "Lk.MO",
                "Intelligence",
                "Beardoil",
                "Book",
                "#FireKlean",
                "Rotex 2",
                "Gyroscope",
                "Gen4 HMK",
                "Ophthalmoscope",
                "AmmoCase",
                "MCase",
                "Keytool",
                "Roler",
                "M995",
                "Factory key",
                "BS",
                "Graphics card",
                "KEK",
                "Grenades",
                "Cordura",
                "Ripstop",
                "Aramid",
                "T H I C C",
                "OPZ",
                "LED X",
                "AESA"
            };


        }

        private bool IsSpecialLootItem(LootItem lootItem)
        {
            if (!(lootItem == null) && lootItem.Item != null)
            {
                string item = lootItem.Item.Name.Localized().ToLower();
                string item2 = lootItem.Item.ShortName.Localized();
                return SpecialLootItems.Contains(item2) || item.Contains(Settings.ItemLookup);
            }
            return false;
        }





        public void FixedUpdate()
        {
            if (Input.GetKeyDown(Settings.ItemCategory))
            {
                if ((int)LootItemLabelState == Enum.GetNames(typeof(LootItemLabelState)).Length - 1)
                {
                    LootItemLabelState = LootItemLabelState.Disabled;
                }
                else
                {
                    LootItemLabelState++;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && LootItemLabelState != LootItemLabelState.Disabled)
            {
                LootItemLabelState--;
            }


            if ((Settings.DrawLootItems || Settings.DrawBodyLoot || Settings.DrawCorpse) && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                if (Time.time >= _nextLootItemCacheTime && Main.GameWorld != null && Main.GameWorld.LootItems != null && Main.MainCamera != null)
                {
                    _gameLootItems.Clear();
                    for (int i = 0; i < Main.GameWorld.LootItems.Count; i++)
                    {
                        LootItem byIndex = Main.GameWorld.LootItems.GetByIndex(i);
                        if (GameUtils.IsLootItemValid(byIndex) && Vector3.Distance(Main.MainCamera.transform.position, byIndex.transform.position) <= Settings.DrawLootItemsDistance)
                        {
                            _gameLootItems.Add(new GameLootItem(byIndex));
                        }
                    }
                    _nextLootItemCacheTime = Time.time + CacheLootItemsInterval;
                }
                foreach (GameLootItem gameLootItem in _gameLootItems)
                {
                    gameLootItem.RecalculateDynamics();
                }
            }


        }

        private void OnGUI()
        {

            if (Settings.DrawLootItems && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                Render.DrawLabel(new Rect(20f, 40f, 200f, 60f), LootItemLabelState.ToString(), Color.cyan);
                if (LootItemLabelState == LootItemLabelState.Disabled)
                    return;
                foreach (GameLootItem gameLootItem in _gameLootItems)
                {
                    if (GameUtils.IsLootItemValid(gameLootItem.LootItem) && gameLootItem.IsOnScreen && gameLootItem.Distance <= Settings.DrawLootItemsDistance)
                    {
                        bool isSpecialLootItem = IsSpecialLootItem(gameLootItem.LootItem);
                        Item rootItem = gameLootItem.LootItem.ItemOwner.RootItem;
                        Color color = CommonColor;

                        string itemname = "Error";
                        int ItemValue = -1;
                        string greaterthanten = "";

                        if (LootItemLabelState == LootItemLabelState.Special && !isSpecialLootItem)
                            continue;
                        else if (LootItemLabelState == LootItemLabelState.GameRare && gameLootItem.LootItem.Item.Template.Rarity < ELootRarity.Rare)
                            continue;
                        else if (LootItemLabelState == LootItemLabelState.GameSuperRare && gameLootItem.LootItem.Item.Template.Rarity < ELootRarity.Superrare)
                            continue;


                        if (isSpecialLootItem)
                        {
                            color = SpecialColor;
                        }
                        else if (gameLootItem.LootItem.Item.QuestItem)
                        {
                            color = QuestColor;
                        }
                        else if (gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Rare)
                        {
                            color = RareColor;
                        }
                        else if (gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Superrare)
                        {
                            color = SuperRareColor;
                        }


                        if (gameLootItem.LootItem.Item.Template != null && gameLootItem.LootItem.Item.ShortName != null)
                        {
                            itemname = gameLootItem.LootItem.Item.ShortName.Localized();
                            ItemValue = gameLootItem.LootItem.Item.Template.CreditsPrice >= 1000 ? gameLootItem.LootItem.Item.Template.CreditsPrice / 1000 : gameLootItem.LootItem.Item.Template.CreditsPrice;
                            greaterthanten = gameLootItem.LootItem.Item.Template.CreditsPrice >= 1000 ? "K" : "";
                        }

                        string label = $"{itemname} [{gameLootItem.FormattedDistance}] [{ItemValue}{greaterthanten}]";

                        Render.DrawString(new Vector2(gameLootItem.ScreenPosition.x - 50f, gameLootItem.ScreenPosition.y), label, color, true, 9);
                    }
                }

            }

            if (Settings.DrawBodyLoot && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                int num = -20;
                foreach (GameLootItem gameLootItem in _gameLootItems)
                {
                    if (GameUtils.IsLootItemValid(gameLootItem.LootItem) && gameLootItem.IsOnScreen && gameLootItem.Distance <= Settings.DrawContainersListDistance)
                    {
                        if (gameLootItem.LootItem.Item.ShortName.Localized().Contains("Default Inventory"))
                        {
                            Item rootItem = gameLootItem.LootItem.ItemOwner.RootItem;
                            if (rootItem.GetAllItems(false).Count() != 0)
                            {
                                string labelList = rootItem.Name.Localized();
                                foreach (Item item in rootItem.GetAllItems(false))
                                {
                                    if (!item.IsAmmo() && !item.IsMagazine() && item.Template.CreditsPrice > 5000 && item.Template._parent != "5448bf274bdc2dfc2f8b456a")
                                    {
                                        int ItemValueList = item.Template.CreditsPrice >= 1000 ? item.Template.CreditsPrice / 1000 : item.Template.CreditsPrice;
                                        string greaterthantenList = item.Template.CreditsPrice >= 1000 ? "K" : "";

                                        labelList = item.ShortName.Localized() + " [" + ItemValueList + greaterthantenList + "]";
                                        ListColour = Color.white;

                                        Render.DrawString(new Vector2(gameLootItem.ScreenPosition.x, gameLootItem.ScreenPosition.y - num), labelList, ListColour, true);
                                        num -= 20;
                                    }
                                }
                            }

                        }

                    }

                }

            }

            if (Settings.DrawCorpse && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                int num = -20;
                foreach (GameLootItem gameLootItem in _gameLootItems)
                {
                    if (GameUtils.IsLootItemValid(gameLootItem.LootItem) && gameLootItem.IsOnScreen && gameLootItem.Distance <= Settings.DrawCorpseDistance)
                    {
                        if (gameLootItem.LootItem.Item.ShortName.Localized().Contains("Default Inventory"))
                        {
                            Render.DrawString(new Vector2(gameLootItem.ScreenPosition.x - 50f, gameLootItem.ScreenPosition.y), $"Body [{gameLootItem.FormattedDistance}] [{BodyValue(gameLootItem)/1000}K]", pink, true , 8);
                        }

                    }

                }

            }
        }

        private int BodyValue(GameLootItem gameLootItem)
        {
            int credit = 0;
            foreach (Item item in gameLootItem.LootItem.ItemOwner.RootItem.GetAllItems())
            {
                credit += item.Template.CreditsPrice;
            }
            return credit;
        }

        public void TPItems()
        {
            if (Settings.TPItems)
            {
                foreach (GameLootItem gameLootItem in _gameLootItems)
                {
                    if (GameUtils.IsLootItemValid(gameLootItem.LootItem) && gameLootItem.Distance <= Settings.TPItemsDistance)
                    {
                        gameLootItem.LootItem.transform.position = Main.MainCamera.transform.position;
                    }
                }
            }
        }

    }
}
/*
 public void FixedUpdate()
        {

            if ((Settings.DrawLootItems || Settings.DrawBodyLoot) && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null)
            {
                if (this.LootItemRarity == (LootItemRarity)Enum.GetNames(typeof(LootItemRarity)).Length - 1)
                {
                    this.LootItemRarity = LootItemRarity.Common;
                }
                else if (Input.GetKeyUp(Settings.ItemCategory))
                {
                    LootItemRarity lootItemRarity = this.LootItemRarity;
                    this.LootItemRarity = lootItemRarity + 1;
                }
                if (Time.time >= this._nextLootItemCacheTime && Main.GameWorld != null && Main.GameWorld.LootItems != null && Main.MainCamera != null)
                {
                    this._gameLootItems.Clear();
                    for (int i = 0; i < Main.GameWorld.LootItems.Count; i++)
                    {
                        LootItem byIndex = Main.GameWorld.LootItems.GetByIndex(i);
                        if (GameUtils.IsLootItemValid(byIndex) && Vector3.Distance(Main.MainCamera.transform.position, byIndex.transform.position) <= Settings.DrawLootItemsDistance)
                        {
                            this._gameLootItems.Add(new GameLootItem(byIndex));
                        }
                    }
                    this._nextLootItemCacheTime = Time.time + ItemESP.CacheLootItemsInterval;
                }
                foreach (GameLootItem gameLootItem in this._gameLootItems)
                {
                    gameLootItem.RecalculateDynamics();
                }
            }


        }


    private void OnGUI()
        {

            if (Settings.DrawLootItems && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {

                foreach (GameLootItem gameLootItem in this._gameLootItems)
                {
                    if (GameUtils.IsLootItemValid(gameLootItem.LootItem) && gameLootItem.IsOnScreen && gameLootItem.Distance <= Settings.DrawLootItemsDistance)
                    {
                        Item rootItem = gameLootItem.LootItem.ItemOwner.RootItem;
                        bool flag = this.IsSpecialLootItem(gameLootItem.LootItem);
                        Color color = ItemESP.CommonColor;
                        if (flag)
                        {
                            color = ItemESP.SpecialColor;
                        }
                        else if (gameLootItem.LootItem.Item.QuestItem)
                        {
                            color = ItemESP.QuestColor;
                        }
                        else if (gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Rare)
                        {
                            color = ItemESP.RareColor;
                        }
                        else if (gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Superrare)
                        {
                            color = ItemESP.SuperRareColor;
                        }

                        int ItemValue = (gameLootItem.LootItem.Item.Template.CreditsPrice > 10000) ? gameLootItem.LootItem.Item.Template.CreditsPrice / 1000 : gameLootItem.LootItem.Item.Template.CreditsPrice;
                        string greaterthanten = (gameLootItem.LootItem.Item.Template.CreditsPrice > 10000) ? "K" : "";
                        string label = gameLootItem.LootItem.Item.ShortName.Localized() + " [" + gameLootItem.FormattedDistance + "]" + " [" + ItemValue + greaterthanten + "]";


                        if ((this.LootItemRarity != LootItemRarity.Special || flag) && (this.LootItemRarity != LootItemRarity.GameRare || gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Rare) && (this.LootItemRarity != LootItemRarity.GameSuperRare || gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Superrare) && (this.LootItemRarity != LootItemRarity.Common || gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Common) && (this.LootItemRarity != LootItemRarity.Common || gameLootItem.LootItem.Item.Template.Rarity == ELootRarity.Common))
                        {
                            Render.DrawString(new Vector2(gameLootItem.ScreenPosition.x - 50f, gameLootItem.ScreenPosition.y), label, color, true);
                        }
                    }
                }
                Render.DrawLabel(new Rect(20f, 40f, 200f, 60f), this.LootItemRarity.ToString(), Color.yellow);
            }

*/
