using BSG.CameraEffects;
using EFT;
using EFT.Ballistics;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.Weather;
using MonoSecurity.Drawing;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MonoSecurity
{
    public class Misc : MonoBehaviour
    {
        private string _hud = string.Empty;
        private static Color FovRing = new Color(21, 118, 187);
        private static NightVision NV;
        private static ThermalVision TV;
        public static List<string> MeleeWeapons;


        private int AmmoCount = -1;
        private int MagAmmoCount = -1;
        private string BulletName = "Empty";
        private string Firemode = "N/A";
        private LayerMask Temp;

        public static DumbHook create_sa_hook;
        public static DumbHook create_pa_hook;
        public static DumbHook create_df_hook;
        public static DumbHook create_db_hook;
        public static DumbHook create_ps_hook;
        public static bool SANothooked = true;
        public static bool DFNotHooked = true;
        public static bool PSNotHooked = true;
        public static bool PANotHooked = true;
        public static bool DBNotHooked = true;
        public static DumbHook create_bind_hook;
        public static DumbHook create_inf_hook;
        public static DumbHook create_spd_hook;
        public static DumbHook create_spd2_hook;
        public static DumbHook create_dev_hook;
        public static DumbHook create_slot_hook;
        public static DumbHook create_gamma_hook;
        public static DumbHook create_gamma2_hook;
        public static DumbHook create_gamma3_hook;
        public static DumbHook create_pocket_hook;
        public static DumbHook create_stand_hook;
        public static DumbHook create_weight_hook;
        public static DumbHook create_melee_hook;
        
        public static bool INFNotHooked = true;
        public static bool SPDNotHooked = true;
        public static bool SPD2NotHooked = true;
        public static bool BINDNotHooked = true;
        public static bool DEVNotHooked = true;
        public static DumbHook create_exam_hook;
        public static bool EXAMNotHooked = false;
        public static bool SLOTNotHooked = true;
        public static bool GAMMANotHooked = true;
        public static bool GAMMANotHooked2 = true;
        public static bool GAMMANotHooked3 = true;
        public static bool PocketNotHooked = true;
        public static bool StandNotHooked = true;
        public static bool WeightNotHooked = true;
        public static bool MeleeNotHooked = true;
        

        public void Update()
        {
            if (GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && Main.LocalPlayer.PointOfView == EPointOfView.FirstPerson && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                this.SuperWeapon();
                this.NoRecoil();
                //Misc.SuperBullet();
                Misc.NoVisor();
                this.MaxStats();
                this.SpeedHack();
                this.PrepareHud();
                Misc.ThermalVison();
                Misc.NightVision();
                this.RemoveSpeedDebuff();
                this.InfiniteStamina();
                this.Hooks();
                this.FullAuto();
                this.InteractionDistance();
                this.Movement();
                Testing();
                this.FastReload();
                this.ExternHooks();
                //ForwardTP();
            }

            if (GameScene.IsLoaded() && GameScene.InMatch())
            {
                this.Flyhack();
            }

            this.HotKeys();
            
        }

        public void Start()
        {
            MeleeWeapons = new List<string>
            {
                "5bffdc370db834001d23eca8",    //6h5 Bayonet
                "5bc9c1e2d4351e00367fbcf0",    //Antique axe
                "57e26fc7245977162a14b800",    //Bars A-2607
                "57e26ea924597715ca604a09",    //Bars A-2607-Damascus
                "5c012ffc0db834001d23f03f",    //Camper axe
                "5bffe7930db834001b734a39",    //Crash axe
                "54491bb74bdc2d09088b4567",    //ER Fulcrum Bayonet
                "5c07df7f0db834001b73588a",    //Freeman crowbar
                "57cd379a24597778e7682ecf",    //Kiba Arms Tactical Tomahawk
                "5bffdd7e0db834001b734a1a",    // M-2 Tactical Sword
                "5bead2e00db834001c062938",    //MPL-50 entrenching tool
                "5c0126f40db834002a125382",    //Red Rebel Ice pick
                "5c010e350db83400232feec7"     //SP-8 Survival Machete
            };
            Temp = EFTHardSettings.Instance.MOVEMENT_MASK;


            Debug.logger.logEnabled = false;
            Debug.unityLogger.logEnabled = false;
            //LogManager.DisableLogging();
            AbstractLogger.IsLogsEnabled = false;
        }

        private void InfiniteStamina()
        {
            if (Settings.InfiniteStamina && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                Main.LocalPlayer.Physical.StaminaRestoreRate = 10000f;
                Main.LocalPlayer.Physical.Stamina.Current = 100f;
                Main.LocalPlayer.Physical.HandsRestoreRate = 1000f;
                Main.LocalPlayer.Physical.HandsStamina.Current = 100f;
            }
        }

        private void HotKeys()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                Settings.DrawGrenades = !Settings.DrawGrenades;
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                Settings.DrawPlayers = !Settings.DrawPlayers;
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Settings.DrawLootItems = !Settings.DrawLootItems;
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                Settings.DrawLootableContainers = !Settings.DrawLootableContainers;
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Settings.DrawExfiltrationPoints = !Settings.DrawExfiltrationPoints;
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                BreachDoor();
            }
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                Settings.SpeedHack = !Settings.SpeedHack;
            }
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                Settings.FlyHack = !Settings.FlyHack;
            }
            if (Input.GetKeyDown(KeyCode.Home))
            {
                this.WeatherChanger();
            }
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                Settings.AlwaysAutomatic = !Settings.AlwaysAutomatic;
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                Settings.FullSprint = !Settings.FullSprint;
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                Main.LocalPlayer.Transform.position += Main.LocalPlayer.Transform.forward * 4f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Main.LocalPlayer.Transform.position += Main.LocalPlayer.Transform.up * -2f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Main.LocalPlayer.Transform.position += Main.LocalPlayer.Transform.up * 2f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Main.LocalPlayer.Transform.position += Main.LocalPlayer.Transform.right * 2f;
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Main.LocalPlayer.Transform.position += Main.LocalPlayer.Transform.up * -2f;
            }
        }

        public void RemoveSpeedDebuff()
        {
            if (Main.LocalPlayer != null && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && Main.LocalPlayer.PointOfView == EPointOfView.FirstPerson && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                //Main.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Armor);
                //Main.LocalPlayer.RemoveMouseSensitivityModifier(Player.EMouseSensitivityModifier.Armor);

                if (Settings.FullSprint)
                {
                    Main.LocalPlayer.EnableSprint(true);
                    Main.LocalPlayer.MovementContext.EnableSprint(true);
                    Main.LocalPlayer.CurrentState.EnableSprint(true, false);
                    //Main.LocalPlayer.MovementContext.PlayerAnimatorEnableInert(true);
                    
                    //Main.LocalPlayer.MovementContext.
                }
            }


        }

        private void FullAuto()
        {
            if (Main.LocalPlayer != null && Main.LocalPlayer.HandsController.Item is Weapon && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.Weapon != null && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null)
                if (Settings.AlwaysAutomatic)
                {
                    Main.LocalPlayer.Weapon.GetItemComponent<FireModeComponent>().FireMode = Weapon.EFireMode.fullauto;
                    return;
                }
                else
                {
                    Main.LocalPlayer.Weapon.GetItemComponent<FireModeComponent>().FireMode = Weapon.EFireMode.single;
                    return;
                }
        }

        private void NoBolt()
        {
            if (Main.LocalPlayer != null && Main.LocalPlayer.HandsController.Item is Weapon && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.Weapon != null && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null)
            {
                if (Settings.NoBolt)
                {
                    Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.BoltAction = false;
                    Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.isBoltCatch = false;
                }
            }
        }



        private void SuperWeapon()
        {
            if (Main.LocalPlayer != null && Main.LocalPlayer.HandsController.Item is Weapon && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.Weapon != null && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null) //&& !MeleeWeapons.Contains(Main.LocalPlayer.HandsController.Item.Name))
            {
                if (Settings.FireRate)
                {
                    Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.bFirerate = Settings.FireRateValue;
                }
                if (Settings.ExperimentalFeatures)
                {
                    Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.Ergonomics = 100;
                    Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.bHearDist = 1;
                    Main.LocalPlayer.ProceduralWeaponAnimation.Mask = EFT.Animations.EProceduralAnimationMask.ForceReaction;
                    Main.LocalPlayer.Weapon.CurrentAmmoTemplate.Tracer = true;
                    Main.LocalPlayer.Weapon.CurrentAmmoTemplate.TracerColor = JsonType.TaxonomyColor.violet;
                    Main.LocalPlayer.Weapon.CurrentAmmoTemplate.PenetrationPower = 1000;
                }    
            }
        }

        public void FastReload()
        {
            if (Main.LocalPlayer != null && Main.LocalPlayer.HandsController.Item is Weapon && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.Weapon != null && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null)
            {
                if (Settings.FastReload)
                {
                    Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.isFastReload = true;
                }
                Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.Template.isFastReload = false;
            }
            
        }

        public void OnGUI()
        {
            if (Settings.DrawWeaponInfo && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                GUIStyle guistyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 25,
                    font = Main.Consolas
                };
                guistyle.normal.textColor = Color.cyan;
                GUI.Label(new Rect(400f, (float)(Screen.height - 48), 512f, 48f), this._hud, guistyle);

            }

            if ((Settings.Aimbot || Settings.SilentAim) && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                Circle.DrawCircleMain(new Vector2((Screen.width) / 2, (Screen.height) / 2), Settings.AimbotFOV, FovRing, 1, true, 22); // * 21f
            }
        }





        private void PrepareHud()
        {
            if (Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null)
            {
                if (Main.LocalPlayer.HandsController.Item is Weapon && Main.LocalPlayer.Weapon != null && Settings.DrawWeaponInfo && Main.LocalPlayer.HandsController.Item.GetCurrentMagazine() != null)
                {
                    AmmoCount = Main.LocalPlayer.HandsController.Item.GetCurrentMagazine().Cartridges.Count + Main.LocalPlayer.Weapon.ChamberAmmoCount;
                    MagAmmoCount = Main.LocalPlayer.HandsController.Item.GetCurrentMagazine().Cartridges.MaxCount;
                    if (Main.LocalPlayer.HandsController.Item.GetCurrentMagazine().Cartridges.Items.FirstOrDefault() != null)
                    {
                        BulletName = Main.LocalPlayer.HandsController.Item.GetCurrentMagazine().Cartridges.Items.FirstOrDefault().ShortName.Localized();
                    }
                    Firemode = Main.LocalPlayer.Weapon.SelectedFireMode.ToString().ToUpper();

                    this._hud = string.Format("{0}/{1} [{2}] {3}", new object[]
                        {
                        AmmoCount,
                        MagAmmoCount,
                        Firemode,
                        BulletName
                        });
                    return;
                }
            }
        }

        private void SpeedHack()
        {
            if (Settings.SpeedHack)
            {
                Time.timeScale = Settings.SpeedValue;
                return;
            }
            Time.timeScale = 1f;
        }

        private void Flyhack()
        {
            int mask = LayerMask.GetMask(new String[]
            {
                "Water",
                "Terrain",
                "HighPolyCollider",
                "TransparentCollider",
                "HitCollider"
            });

            if (Settings.FloatOff)
            {
                EFTHardSettings.Instance.MOVEMENT_MASK = Temp;
            }
            else
            {
                EFTHardSettings.Instance.MOVEMENT_MASK = LayerMask.GetMask(new String[]
                {
                "Water",
                "Terrain",
                "HighPolyCollider",
                "TransparentCollider",
                "HitCollider"
                });
            }

            if (Settings.FlyHack)
            {
                int vis_mask = 1 << 12 | 1 << 16;
                LayerMask air = vis_mask;
                Main.LocalPlayer.MovementContext.IsGrounded = true;
                //Main.LocalPlayer.CharacterControllerCommon.ShouldStickToGround = false;
                Main.LocalPlayer.MovementContext.FreefallTime = Settings.FlySpeed;
            }
        }

        private void InteractionDistance()
        {
            if (Settings.LootRaycast)
            {
                EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE = Settings.LootRaycastDistance;
            }
            else
            {
                EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE = 1f;
            }

            if (Settings.DoorRaycast)
            {
                EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE = Settings.DoorRaycastDistance;
            }
            else
            {
                EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE = 0.75f;
            }

            if (Settings.PlayerRaycast)
            {
                EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE = Settings.PlayerRaycastDistance;
            }
            else
            {
                EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE = 2.5f;
            }
        }

        private void Movement()
        {
            if (Settings.MovementMod)
            {
                Main.LocalPlayer.HandsAnimator.SetPlayerState(ObjectInHandsAnimator.PlayerState.None);
                Main.LocalPlayer.MovementContext.CurrentState.Name = EPlayerState.None;
                Main.LocalPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = false;
                Main.LocalPlayer.ProceduralWeaponAnimation.DrawEffectorEnabled = false;
                Main.LocalPlayer.MovementContext.CovertEfficiency = 1.5f;
                foreach (EPhysicalCondition PhysicalCondition in Enum.GetValues(typeof(EPhysicalCondition)))
                {
                    if (PhysicalCondition != EPhysicalCondition.None || PhysicalCondition != EPhysicalCondition.OnPainkillers)
                    {
                        Main.LocalPlayer.MovementContext.SetPhysicalCondition(PhysicalCondition, false);
                    }
                }

                foreach (EFT.Player.ESpeedLimit speedlimit in Enum.GetValues(typeof(EFT.Player.ESpeedLimit)))
                {
                    if (speedlimit != Player.ESpeedLimit.SurfaceNormal)
                    {
                        Main.LocalPlayer.RemoveStateSpeedLimit(speedlimit);
                    }  
                }
            }
        }

        public void Testing()
        {
            if (Settings.Testings)
            {
                //Main.LocalPlayer.MovementContext.PlayerAnimatorSetStationary(true);
                EFTHardSettings.Instance.DelayToOpenContainer = 0f;
                EFTHardSettings.Instance.PICKUP_DELAY = 0f;
                EFTHardSettings.Instance.ThrowLootMakeVisibleDelay = 0f;
                //EFTHardSettings.Instance.GrenadeForce = 50f;
            }
            else
            {
                EFTHardSettings.Instance.PICKUP_DELAY = 1f;
                EFTHardSettings.Instance.DelayToOpenContainer = 0.5f;
                EFTHardSettings.Instance.ThrowLootMakeVisibleDelay = 0.15f;
                EFTHardSettings.Instance.GrenadeForce = 20f;

            }
        }


        private void NoRecoil()
        {
            if (Main.LocalPlayer == null)
            {
                return;
            }
            if (Settings.NoRecoil)
            {
                Main.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.Shootingg.RecoilStrengthXy = new Vector2(0f, 0f);
                Main.LocalPlayer.ProceduralWeaponAnimation.Shootingg.RecoilStrengthZ = new Vector2(0f, 0f);
                Main.LocalPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = false;
                Main.LocalPlayer.ProceduralWeaponAnimation._shouldMoveWeaponCloser = false;
                Main.LocalPlayer.ProceduralWeaponAnimation.MotionReact.SwayFactors.x = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.MotionReact.SwayFactors.y = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.MotionReact.SwayFactors.z = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.Breath.Intensity = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.Walk.Intensity = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Stiffness = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = 0f;
                Main.LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = 0f;
                return;
            }
            Main.LocalPlayer.ProceduralWeaponAnimation.Breath.Intensity = 1f;
            Main.LocalPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = true;
            Main.LocalPlayer.ProceduralWeaponAnimation.Walk.Intensity = 1f;
            Main.LocalPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = 1f;
            Main.LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = 1f;
        }


        private static void NoVisor()
        {

            if (!(Main.LocalPlayer == null) && !(Main.MainCamera == null))
            {
                if (Settings.NoVisor)
                {
                    Main.MainCamera.GetComponent<VisorEffect>().Intensity = 0f;
                    Main.MainCamera.GetComponent<VisorEffect>().enabled = true;
                }
                else
                {
                    Main.MainCamera.GetComponent<VisorEffect>().Intensity = 1f;
                    Main.MainCamera.GetComponent<VisorEffect>().enabled = true;
                }
            }


        }

        private static void ThermalVison()
        {
            if (Main.LocalPlayer == null || Main.MainCamera == null)
            {
                return;
            }
            if (Settings.ThermalVison)
            {
                TV = Main.MainCamera.GetComponent<ThermalVision>();
                TV.IsFpsStuck = false;
                TV.IsGlitch = false;
                TV.IsMotionBlurred = false;
                TV.IsNoisy = false;
                TV.IsPixelated = false;
                TV.ThermalVisionUtilities.DepthFade = 0f;
                TV.ThermalVisionUtilities.NoiseParameters.NoiseIntensity = 0f;
                TV.ThermalVisionUtilities.ValuesCoefs.SpecularCoef = 0.01f;
                TV.ThermalVisionUtilities.ValuesCoefs.RampShift = 0f;
                TV.On = true;
                TV.enabled = true;
                return;
            }
            Main.MainCamera.GetComponent<ThermalVision>().On = false;
            Main.MainCamera.GetComponent<ThermalVision>().enabled = true;
        }

        private static void NightVision()
        {
            if (Main.LocalPlayer == null || Main.MainCamera == null)
            {
                return;
            }
            if (Settings.NightVison)
            {
                NV = Main.MainCamera.GetComponent<NightVision>();
                NV.DiffuseIntensity = 0f;
                NV.TextureMask.Color = new Color(0f, 0f, 0f, 0f);
                NV.TextureMask.Stretch = false;
                NV.TextureMask.Size = 0f;
                NV.Intensity = 0f;
                NV.NoiseIntensity = 0f;
                NV.StartSwitch(true);
                NV.enabled = true;
                return;
            }
            Main.MainCamera.GetComponent<NightVision>().StartSwitch(false);
            Main.MainCamera.GetComponent<NightVision>().enabled = true;
        }


        private void MaxStats()
        {
            if (Settings.MaxSkills && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                for (int i = 0; i < Main.LocalPlayer.Skills.Skills.Count(); i++)
                {
                    if (Main.LocalPlayer.Skills.Skills[i].Level != 51)
                    {
                        Main.LocalPlayer.Skills.Skills[i].SetLevel(51);
                    }

                }

                
            }
        }

        

        public void ExamineAll(Player Player)
        {
            Player.Profile.ExamineAll();
        }

        public void BreachDoor()
        {
            foreach (Door door in GameObject.FindObjectsOfType<Door>())
            {
                if (door == null)
                    continue;

                if (door.DoorState != EDoorState.Locked)
                    continue;


                if ((door.DoorState == EDoorState.Open) || Vector3.Distance(Camera.main.transform.position, door.transform.position) > 4f)
                    continue;

                door.enabled = true;
                door.CanBeBreached = true;
                door.DoorState = EDoorState.Shut;
            }
        }




        public void WeatherChanger()
        {
            if (Main.LocalPlayer != null && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && Main.LocalPlayer.PointOfView == EPointOfView.FirstPerson && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                TOD_Sky.Instance.Components.Time.GameDateTime = null;
                TOD_Sky Sky_Obj = (TOD_Sky)FindObjectOfType(typeof(TOD_Sky));
                WeatherController.Instance.RainController.SetIntensity(0f);
                WeatherController.Instance.GlobalFogOvercast = 0f;
                WeatherController.Instance.LightningSummonBandWidth = 0f;
                WeatherController.Instance.RainController.enabled = false;
                Sky_Obj.Cycle.Hour = 12f;
            }
        }

        public void ExternHooks()
        {
            if (EXAMNotHooked)
            {
                //Main.LocalPlayer.ActiveHealthController.GetType().GetMethod("HandleFall");

                create_exam_hook = new DumbHook();
                create_exam_hook.Init(typeof(Item).GetMethod("get_ExaminedByDefault"), typeof(B1ghook).GetMethod("InfHk")); //typeof(Item).Assembly.GetType("\uE1B4")
                create_exam_hook.Hook();
                EXAMNotHooked = false;
            }

            if (SLOTNotHooked)
            {
                Type InvController = typeof(EFT.UI.TasksScreen).GetField("_inventoryController", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).FieldType;
                if (InvController != null)
                {
                    create_slot_hook = new DumbHook();
                    create_slot_hook.Init(InvController.GetMethod("IsAllowedToSeeSlot"), typeof(B1ghook).GetMethod("SlotHk"));
                    create_slot_hook.Hook();
                    SLOTNotHooked = false;
                }
            }

            if (GAMMANotHooked)
            {
                create_gamma_hook = new DumbHook();
                create_gamma_hook.Init(typeof(CantPutIntoDuringRaidComponent).GetMethod("CanPutInto"), typeof(B1ghook).GetMethod("InfHk"));
                create_gamma_hook.Hook();
                GAMMANotHooked = false;
            }

            if (GAMMANotHooked2)
            {
                create_gamma2_hook = new DumbHook();
                create_gamma2_hook.Init(typeof(CantRemoveFromSlotsDuringRaidComponent).GetMethod("CanRemoveFromSlotDuringRaid"), typeof(B1ghook).GetMethod("GammaHk"));
                create_gamma2_hook.Hook();
                GAMMANotHooked2 = false;
            }

            /*
            if (PocketNotHooked)
            {
                create_pocket_hook = new DumbHook();
                create_pocket_hook.Init(typeof(Item).GetMethod("get_NotShownInSlot"), typeof(B1ghook).GetMethod("KeyHk"));
                create_pocket_hook.Hook();
                PocketNotHooked = false;
            }
            */

        }


        public void Hooks()
        {
            if (Settings.SilentAim)
            {
                if (SANothooked)
                {
                    create_sa_hook = new DumbHook();
                    create_sa_hook.Init(typeof(BallisticsCalculator).GetMethod("CreateShot"), typeof(B1ghook).GetMethod("SilentaimHk"));
                    create_sa_hook.Hook();
                    SANothooked = false;
                }
            }


            if (Settings.ExperimentalFeatures)
            {
                if (PANotHooked)
                {
                    create_pa_hook = new DumbHook();
                    create_pa_hook.Init(typeof(BodyPartCollider).GetMethod("IsPenetrated"), typeof(B1ghook).GetMethod("PenHk"));
                    create_pa_hook.Hook();
                    PANotHooked = false;
                }

                if (DFNotHooked)
                {
                    create_df_hook = new DumbHook();
                    create_df_hook.Init(typeof(BodyPartCollider).GetMethod("Deflects"), typeof(B1ghook).GetMethod("DefHk"));
                    create_df_hook.Hook();
                    DFNotHooked = false;
                }

                if (SPDNotHooked)
                {
                    create_spd_hook = new DumbHook();
                    create_spd_hook.Init(typeof(Player.FirearmController).GetMethod("get_SpeedFactor"), typeof(B1ghook).GetMethod("SpdHk"));
                    create_spd_hook.Hook();
                    SPDNotHooked = false;
                }

                if (SPD2NotHooked)
                {
                    create_spd2_hook = new DumbHook();
                    create_spd2_hook.Init(typeof(Weapon).GetMethod("get_SpeedFactor"), typeof(B1ghook).GetMethod("SpdHk2"));
                    create_spd2_hook.Hook();
                    SPD2NotHooked = false;
                }
                /*
                if (BINDNotHooked)
                {
                    Type InvController = typeof(EFT.UI.TasksScreen).GetField("_inventoryController", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).FieldType;
                    if (InvController != null)
                    {
                        create_bind_hook = new DumbHook();
                        //GameUtils.GetType(0x02000FCD) .GetField("_inventoryController") .GetMethod("IsAtBindablePlace")
                        create_bind_hook.Init(InvController.GetMethod("IsAtBindablePlace"), typeof(B1ghook).GetMethod("BindHk"));
                        create_bind_hook.Hook();
                        BINDNotHooked = false;
                    }
                }
                */
                if (INFNotHooked)
                {
                    Type InvController = typeof(EFT.UI.TasksScreen).GetField("_inventoryController", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).FieldType;
                    if (InvController != null)
                    {
                        create_inf_hook = new DumbHook();
                        create_inf_hook.Init(InvController.GetMethod("CanStartNewSearchOperation"), typeof(B1ghook).GetMethod("InfHk"));
                        create_inf_hook.Hook();
                        INFNotHooked = false;

                    }
                }


                if (StandNotHooked)
                {
                    Type MovementContext = typeof(MovementState).GetField("MovementContext", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).FieldType;
                    if (MovementContext != null)
                    {
                        create_stand_hook = new DumbHook();
                        create_stand_hook.Init(MovementContext.GetMethod("CanStandAt"), typeof(B1ghook).GetMethod("StandHk"));
                        create_stand_hook.Hook();
                        StandNotHooked = false;
                    }
                }

                if (WeightNotHooked)
                {
                    Type Physical = typeof(EFT.Player).GetField("Physical", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).FieldType;
                    if (Physical != null)
                    {
                        create_weight_hook = new DumbHook();
                        create_weight_hook.Init(Physical.GetMethod("UpdateWeightLimits"), typeof(B1ghook).GetMethod("WeightHk"));
                        create_weight_hook.Hook();
                        WeightNotHooked = false;
                    }
                }

                if (MeleeNotHooked)
                {
                    Type Physical = typeof(EFT.Player).GetField("Physical", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).FieldType;
                    if (Physical != null)
                    {
                        create_melee_hook = new DumbHook();
                        create_melee_hook.Init(Physical.GetMethod("get_MeleeSpeed"), typeof(B1ghook).GetMethod("MeleeHk"));
                        create_melee_hook.Hook();
                        MeleeNotHooked = false;
                    }
                }
            }

            if (Settings.InstantDoorBreach)
            {
                if (DBNotHooked)
                {
                    create_db_hook = new DumbHook();
                    create_db_hook.Init(typeof(Door).GetMethod("BreachSuccessRoll"), typeof(B1ghook).GetMethod("DoorHk"));
                    create_db_hook.Hook();
                    DBNotHooked = false;
                }
            }

            if (Settings.Testings)
            {
                /*
                if (STSNotHooked)
                {
                    create_static_hook = new DumbHook();
                    create_static2_hook = new DumbHook();

                    create_static_hook.Init(typeof(BetterAudio).Assembly.GetType("\uE6CB").GetMethod("get_Succeed"), typeof(B1ghook).GetMethod("InfHk"));
                    create_static2_hook.Init(typeof(BetterAudio).Assembly.GetType("\uE6CB").GetMethod("ValidateCertificate"), typeof(B1ghook).GetMethod("CertHk"));

                    create_static_hook.Hook();
                    create_static2_hook.Hook();
                    STSNotHooked = false;
                }

                if (NAMENotHooked)
                {
                    //Main.LocalPlayer.ActiveHealthController.GetType().GetMethod("HandleFall");x
                    if (Main.LocalPlayer != null && Main.LocalPlayer.ActiveHealthController != null)
                    {
                        create_name_hook = new DumbHook();
                        create_name_hook.Init(typeof(EFT.Player).GetField("ActiveHealthController").FieldType.GetMethod("HandleFall"), typeof(B1ghook).GetMethod("FallHk")); //typeof(Item).Assembly.GetType("\uE1B4")
                        create_name_hook.Hook();
                        NAMENotHooked = false;
                    }
                    
                }
                */


                
            }

            /*
            if (Settings.DevMode)
            {
                if (DEVNotHooked)
                {
                    create_dev_hook = new DumbHook();
                    create_dev_hook.Init(typeof(AbstractSession).GetMethod("get_MemberCategory"), typeof(B1ghook).GetMethod("DevHk"));
                    create_dev_hook.Hook();
                    DEVNotHooked = false;
                }
            }
            */

            /*
                
                if (Settings.GrenadeAimbot)
                {
                    if (GANotHooked)
                    {
                        create_ga_hook = new DumbHook();
                        create_ga_hook.Init(typeof(Grenade).GetMethod("Explosion"), typeof(b1ghook).GetMethod("grenadeHk"));
                        create_ga_hook.Hook();
                        GANotHooked = false;
                    }
                }
            */
        }

    }
}
