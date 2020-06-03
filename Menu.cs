using EFT;
using MonoSecurity.Drawing;
using System;
using UnityEngine;

namespace MonoSecurity
{
    public class Menu : MonoBehaviour
    {
        private Rect _mainWindow;

        private Rect _playerVisualWindow;

        private Rect _miscVisualWindow;

        private Rect _aimbotVisualWindow;

        private Rect _miscFeatureslVisualWindow;

        private Rect _waterMark;

        private Rect _playerCount;

        private bool _visible = false;

        private bool _playerEspVisualVisible;

        private bool _miscVisualVisible;

        private bool _aimbotVisualVisible;

        private bool _miscFeatureslVisible;

        private string GameStatus = "";

        private readonly string watermark = "Juden Buster";

        private void Start()
        {
            this._mainWindow = new Rect(20f, 60f, 250f, 150f);
            this._playerVisualWindow = new Rect(20f, 220f, 250f, 200f);
            this._miscVisualWindow = new Rect(20f, 260f, 250f, 200f);
            this._aimbotVisualWindow = new Rect(20f, 260f, 250f, 150f);
            this._miscFeatureslVisualWindow = new Rect(20f, 260f, 250f, 200f);
            this._playerCount = new Rect(20f, 60f, 200f, 60f);
            this._waterMark = new Rect(20f, 20f, 1000f, 500f);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                this._visible = !this._visible;
            }
        }

        private void OnGUI()
        {
            if (!GameScene.InMatch())
            {
                GameStatus = "Main menu";
            }
            else if (MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                GameStatus = "Waiting for game to start";
            }
            else if (GameScene.IsLoaded() && GameScene.InMatch() && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                GameStatus = "Ingame";
            }


            Render.DrawLabel(this._waterMark, $"{this.watermark}: {GameStatus}, Vox is a kike", Color.cyan);

            if (GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && Main.GamePlayers.Count > -1)
            {
                int playerCount = 0;
                foreach (GamePlayer gamePlayer in Main.GamePlayers)
                {
                    if (!gamePlayer.IsAI && gamePlayer.Player != Main.LocalPlayer && !GameUtils.IsInYourGroup(gamePlayer.Player))
                    {
                        playerCount++;
                    }
                }
                Render.DrawLabel(this._playerCount, $"Kikes: {playerCount}", Color.cyan);
            }

            if (!this._visible)
            {
                return;
            }
            this._mainWindow = GUILayout.Window(0, this._mainWindow, new GUI.WindowFunction(this.RenderUi), this.watermark, Array.Empty<GUILayoutOption>());
            if (this._playerEspVisualVisible)
            {
                this._playerVisualWindow = GUILayout.Window(1, this._playerVisualWindow, new GUI.WindowFunction(this.RenderUi), "Player Visual", Array.Empty<GUILayoutOption>());
            }
            if (this._miscVisualVisible)
            {
                this._miscVisualWindow = GUILayout.Window(2, this._miscVisualWindow, new GUI.WindowFunction(this.RenderUi), "Misc Visual", Array.Empty<GUILayoutOption>());
            }
            if (this._aimbotVisualVisible)
            {
                this._aimbotVisualWindow = GUILayout.Window(3, this._aimbotVisualWindow, new GUI.WindowFunction(this.RenderUi), "Aimbot", Array.Empty<GUILayoutOption>());
            }
            if (this._miscFeatureslVisible)
            {
                this._miscFeatureslVisualWindow = GUILayout.Window(4, this._miscFeatureslVisualWindow, new GUI.WindowFunction(this.RenderUi), "Misc", Array.Empty<GUILayoutOption>());
            }
        }

        private void RenderUi(int id)
        {


            switch (id)
            {
                case 0:
                    GUILayout.Label("Insert For Menu", Array.Empty<GUILayoutOption>());
                    if (GUILayout.Button("Player ESP", Array.Empty<GUILayoutOption>()))
                    {
                        this._playerEspVisualVisible = !this._playerEspVisualVisible;
                    }
                    if (GUILayout.Button("Loot/Extracts/Containers ESP", Array.Empty<GUILayoutOption>()))
                    {
                        this._miscVisualVisible = !this._miscVisualVisible;
                    }
                    if (GUILayout.Button("Aimbot", Array.Empty<GUILayoutOption>()))
                    {
                        this._aimbotVisualVisible = !this._aimbotVisualVisible;
                    }
                    if (GUILayout.Button("Misc", Array.Empty<GUILayoutOption>()))
                    {
                        this._miscFeatureslVisible = !this._miscFeatureslVisible;
                    }

                    break;
                case 1:
                    Settings.DrawPlayers = GUILayout.Toggle(Settings.DrawPlayers, "Draw Players (F8)", Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayerList = GUILayout.Toggle(Settings.DrawPlayerList, "Display list of players", Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayerBox = GUILayout.Toggle(Settings.DrawPlayerBox, "Draw Player Box", Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayerName = GUILayout.Toggle(Settings.DrawPlayerName, "Draw Player Name", Array.Empty<GUILayoutOption>());
                    Settings.HideNames = GUILayout.Toggle(Settings.HideNames, "Hide Real Player Name", Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayerLine = GUILayout.Toggle(Settings.DrawPlayerLine, "Draw Snap Line", Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayerHeadLine = GUILayout.Toggle(Settings.DrawPlayerHeadLine, "Draw Player Head Line", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Player Distance {0} m", (int)Settings.DrawPlayersDistance), Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayersDistance = GUILayout.HorizontalSlider(Settings.DrawPlayersDistance, 0f, 2000f, Array.Empty<GUILayoutOption>());
                    Settings.DrawPlayerSkeleton = GUILayout.Toggle(Settings.DrawPlayerSkeleton, "Draw Player Skeleton", Array.Empty<GUILayoutOption>());
                    Settings.DrawScavSkeleton = GUILayout.Toggle(Settings.DrawScavSkeleton, "Draw Scav Skeleton", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Skeleton Distance {0} m", (int)Settings.DrawSkeletonDistance), Array.Empty<GUILayoutOption>());
                    Settings.DrawSkeletonDistance = GUILayout.HorizontalSlider(Settings.DrawSkeletonDistance, 0f, 2000f, Array.Empty<GUILayoutOption>());
                    Settings.DrawBodyLoot = GUILayout.Toggle(Settings.DrawBodyLoot, "Draw Body Loot", Array.Empty<GUILayoutOption>());
                    Settings.DrawGrenades = GUILayout.Toggle(Settings.DrawGrenades, "Draw Grenades (F7) 50m", Array.Empty<GUILayoutOption>());
                    break;
                case 2:
                    Settings.DrawLootItems = GUILayout.Toggle(Settings.DrawLootItems, "Draw Loot Items (F9)", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Loot Item Distance {0} m", (int)Settings.DrawLootItemsDistance), Array.Empty<GUILayoutOption>());
                    Settings.DrawLootItemsDistance = GUILayout.HorizontalSlider(Settings.DrawLootItemsDistance, 0f, 1000f, Array.Empty<GUILayoutOption>());
                    Settings.DrawLootableContainers = GUILayout.Toggle(Settings.DrawLootableContainers, "Draw Containers (F10)", Array.Empty<GUILayoutOption>());
                    Settings.DrawContainersContent = GUILayout.Toggle(Settings.DrawContainersContent, "Draw Containers Content", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Container Distance {0} m", (int)Settings.DrawLootableContainersDistance), Array.Empty<GUILayoutOption>());
                    Settings.DrawLootableContainersDistance = GUILayout.HorizontalSlider(Settings.DrawLootableContainersDistance, 0f, 1000f, Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Container Content {0} m", (int)Settings.DrawContainersListDistance), Array.Empty<GUILayoutOption>());
                    Settings.DrawContainersListDistance = GUILayout.HorizontalSlider(Settings.DrawContainersListDistance, 0f, 1000f, Array.Empty<GUILayoutOption>());
                    Settings.ItemLookup = GUILayout.TextField(Settings.ItemLookup, Array.Empty<GUILayoutOption>());
                    Settings.DrawExfiltrationPoints = GUILayout.Toggle(Settings.DrawExfiltrationPoints, "Draw Exits (F11)", Array.Empty<GUILayoutOption>());
                    Settings.DrawCorpse = GUILayout.Toggle(Settings.DrawCorpse, "Draw Bodies", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Body distance {0} m", (int)Settings.DrawCorpseDistance), Array.Empty<GUILayoutOption>());
                    Settings.DrawCorpseDistance = GUILayout.HorizontalSlider(Settings.DrawCorpseDistance, 0f, 1000f, Array.Empty<GUILayoutOption>());
                    break;
                case 3:
                    Settings.Aimbot = GUILayout.Toggle(Settings.Aimbot, "Aimbot (LCtrl)", Array.Empty<GUILayoutOption>());
                    Settings.SilentAim = GUILayout.Toggle(Settings.SilentAim, "Silent Aimbot", Array.Empty<GUILayoutOption>());
                    Settings.DrawAimbotPoint = GUILayout.Toggle(Settings.DrawAimbotPoint, "Aimbot Point", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Aimbot FOV {0}", (int)Settings.AimbotFOV/5.555555f), Array.Empty<GUILayoutOption>());
                    Settings.AimbotFOV = GUILayout.HorizontalSlider(Settings.AimbotFOV, 0f, 1000f, Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Aimbot Distance {0} m", Settings.AimBotDistance), Array.Empty<GUILayoutOption>());
                    Settings.AimBotDistance = GUILayout.HorizontalSlider(Settings.AimBotDistance, 0f, 1000f, Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Bone name: {0}", GameUtils.BoneName(GameUtils.AimBoneTarget())), Array.Empty<GUILayoutOption>());
                    Settings.BoneChange = (int)GUILayout.HorizontalSlider(Settings.BoneChange, 1f, 3f, Array.Empty<GUILayoutOption>());
                    break;
                case 4:
                    Settings.MaxSkills = GUILayout.Toggle(Settings.MaxSkills, "Max Skills", Array.Empty<GUILayoutOption>());
                    Settings.InfiniteStamina = GUILayout.Toggle(Settings.InfiniteStamina, "Infinite Stamina", Array.Empty<GUILayoutOption>());
                    Settings.NoVisor = GUILayout.Toggle(Settings.NoVisor, "No Visor", Array.Empty<GUILayoutOption>());
                    Settings.ThermalVison = GUILayout.Toggle(Settings.ThermalVison, "Thermal Vison", Array.Empty<GUILayoutOption>());
                    Settings.NightVison = GUILayout.Toggle(Settings.NightVison, "Night Vison", Array.Empty<GUILayoutOption>());
                    Settings.SpeedHack = GUILayout.Toggle(Settings.SpeedHack, string.Format("Speedhack {0} (Numpad Plus)", Settings.SpeedValue), Array.Empty<GUILayoutOption>());
                    Settings.SpeedValue = GUILayout.HorizontalSlider(Settings.SpeedValue, 1f, 3f, Array.Empty<GUILayoutOption>());
                    Settings.FastReload = GUILayout.Toggle(Settings.FastReload, "Fast Reload", Array.Empty<GUILayoutOption>());
                    Settings.AlwaysAutomatic = GUILayout.Toggle(Settings.AlwaysAutomatic, "Always Automatic", Array.Empty<GUILayoutOption>());
                    Settings.NoBolt = GUILayout.Toggle(Settings.NoBolt, "No Bolt Action", Array.Empty<GUILayoutOption>());
                    Settings.FireRate = GUILayout.Toggle(Settings.FireRate, string.Format("Change Fire Rate {0}", Settings.FireRateValue), Array.Empty<GUILayoutOption>());
                    Settings.FireRateValue = (int)GUILayout.HorizontalSlider((float)Settings.FireRateValue, 1000f, 3000f, Array.Empty<GUILayoutOption>());
                    Settings.DrawWeaponInfo = GUILayout.Toggle(Settings.DrawWeaponInfo, "Draw Weapon Info", Array.Empty<GUILayoutOption>());
                    Settings.NoRecoil = GUILayout.Toggle(Settings.NoRecoil, "No Recoil", Array.Empty<GUILayoutOption>());
                    Settings.ExperimentalFeatures = GUILayout.Toggle(Settings.ExperimentalFeatures, "Experimental Features", Array.Empty<GUILayoutOption>());
                    GUILayout.Label(string.Format("Fly Speed: {0}", Settings.FlySpeed), Array.Empty<GUILayoutOption>());
                    Settings.FlySpeed = GUILayout.HorizontalSlider(Settings.FlySpeed, -5, 0, Array.Empty<GUILayoutOption>());
                    Settings.FlyHack = GUILayout.Toggle(Settings.FlyHack, "Fly hack (Page Up)", Array.Empty<GUILayoutOption>());
                    Settings.FloatOff = GUILayout.Toggle(Settings.FloatOff, "Use old layermask", Array.Empty<GUILayoutOption>());
                    Settings.FullSprint = GUILayout.Toggle(Settings.FullSprint, "Full sprint", Array.Empty<GUILayoutOption>());
                    Settings.InstantDoorBreach = GUILayout.Toggle(Settings.InstantDoorBreach, "Instant Door Breach", Array.Empty<GUILayoutOption>());
                    Settings.MovementMod = GUILayout.Toggle(Settings.MovementMod, "Movement tweaks", Array.Empty<GUILayoutOption>());
                    Settings.Testings = GUILayout.Toggle(Settings.Testings, "Testing ideas", Array.Empty<GUILayoutOption>());
                    Settings.DoorRaycast = GUILayout.Toggle(Settings.DoorRaycast, string.Format("Change Door Raycast {0}", Settings.DoorRaycastDistance), Array.Empty<GUILayoutOption>());
                    Settings.DoorRaycastDistance = GUILayout.HorizontalSlider(Settings.DoorRaycastDistance, 1f, 1000f, Array.Empty<GUILayoutOption>());
                    Settings.LootRaycast = GUILayout.Toggle(Settings.LootRaycast, string.Format("Change loot Raycast {0}", Settings.LootRaycastDistance), Array.Empty<GUILayoutOption>());
                    Settings.LootRaycastDistance = GUILayout.HorizontalSlider(Settings.LootRaycastDistance, 1f, 1000f, Array.Empty<GUILayoutOption>());
                    Settings.PlayerRaycast = GUILayout.Toggle(Settings.PlayerRaycast, string.Format("Change Player Raycast {0}", Settings.PlayerRaycastDistance), Array.Empty<GUILayoutOption>());
                    Settings.PlayerRaycastDistance = GUILayout.HorizontalSlider(Settings.PlayerRaycastDistance, 1f, 1000f, Array.Empty<GUILayoutOption>());
                    break;
            }
            GUI.DragWindow();
        }

    }
}
//internal static float LootRaycastDistance = 1f;

//internal static float lootRaycastDistance = 1f;

//internal static float PlayerRaycastDistance = 1f;