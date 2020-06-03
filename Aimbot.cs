using EFT;
using EFT.InventoryLogic;
using MonoSecurity.Drawing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MonoSecurity
{
    public class Aimbot : MonoBehaviour
    {
        private IEnumerable<GamePlayer> _targetList;

        public GamePlayer _target;

        private Vector3 _aimTarget = Vector3.zero;

        private Color color;

        private static LayerMask layer_mask = LayerMask.GetMask(new string[] {
                    "Terrain",
                    "HighPolyCollider",
                    "Player",
                    "Grass",
                    "Foliage"
        });

        public GamePlayer GetTarget()
        {
            this._targetList = from p in Main.GamePlayers
                               where !p.Player.IsYourPlayer() && GameUtils.IsPlayerAlive(p.Player) && !GameUtils.IsInYourGroup(p.Player)
                               select p;
            this._targetList = from p in this._targetList
                               orderby p.Fov, p.Distance
                               select p;
            foreach (GamePlayer gamePlayer in this._targetList)
            {
                if (gamePlayer.Distance <= (float)Settings.AimBotDistance && gamePlayer.Fov <= Settings.AimbotFOV && gamePlayer.IsOnScreen)
                {
                    return gamePlayer;
                }
            }
            return null;
        }

        public void OnGUI()
        {
            if (GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null && !MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance.IsBackgroundBlackActive)
            {
                if (Main.LocalPlayer.HandsController.Item is Weapon && Main.LocalPlayer.Weapon != null)
                {
                    color = Color.cyan;
                    int boneID = GameUtils.AimBoneTarget();
                    if (this._target != null && Settings.DrawAimbotPoint)
                    {
                        Vector3 Aimtarget = GameUtils.WorldPointToScreenPoint((GameUtils.GetBonePosByID(this._target.Player, boneID)));
                        color = CamisVisible(_target.Player, GameUtils.GetBonePosByID(this._target.Player, boneID)) ? Color.green : Color.red;
                        Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), new Vector2(Aimtarget.x, Aimtarget.y), 1f, color); //new Vector2(_target.HeadScreenPosition.x, _target.HeadScreenPosition.y)
                    }

                    Render.DrawSwastika(color);


                    if (Settings.Aimbot && this._target == null)
                    {
                        this._target = this.GetTarget();

                        if (this._target != null && Input.GetKey(Settings.AimbotKey))
                        {
                            Vector3 bonePosByID = GameUtils.GetBonePosByID(this._target.Player, Settings.AimBotBone);
                            float bullettime = this._target.Distance / Main.LocalPlayer.GetComponent<Player.FirearmController>().Item.CurrentAmmoTemplate.InitialSpeed; //Main.LocalPlayer.Weapon.CurrentAmmoTemplate.InitialSpeed;
                            bonePosByID += this._target.Player.Velocity * bullettime;
                            bonePosByID -= Main.LocalPlayer.Velocity * Time.deltaTime;
                            this._aimTarget = bonePosByID;


                            Vector3 eulerAngles = Quaternion.LookRotation((this._aimTarget - Main.MainCamera.transform.position).normalized).eulerAngles;
                            if (eulerAngles.x > 180f)
                            {
                                eulerAngles.x -= 360f;
                            }
                            Main.LocalPlayer.MovementContext.Rotation = new Vector2(eulerAngles.y, eulerAngles.x);

                        }
                    }
                    else
                    {
                        this._target = null;
                    }
                }
            }
        }


        /*
        public static GamePlayer GetTarget(IEnumerable<GamePlayer> _targetList)
        {
            _targetList = from p in Main.GamePlayers
                          where !(p.Player.IsYourPlayer()) && GameUtils.IsPlayerAlive(p.Player) //&& !GameUtils.IsInYourGroup(p.Player)
                          select p;
            _targetList = from p in _targetList
                          orderby p.Fov, p.Distance
                          select p;
            foreach (GamePlayer gamePlayer in _targetList)
            {
                if (gamePlayer.Distance <= (float)Settings.AimBotDistance && gamePlayer.Fov <= Settings.AimbotFOV && IsVisible(gamePlayer.Player.PlayerBones.Head.position)
                {
                    return gamePlayer;
                }
            }
            return null;
        }
        */

        public static bool IsVisible(Vector3 Position)
        {
            RaycastHit raycastHit;
            return Physics.Linecast(Aimbot.GetShootPos(), Position, out raycastHit, Aimbot.layer_mask) && raycastHit.transform.name.Contains("Human"); // && raycastHit.transform.name.Contains("Human")
        }

        public static bool CamisVisible(Player player, Vector3 AimboneVec)
        {
            int vis_mask = 1 << 12 | 1 << 16;
            RaycastHit raycastHit;
            return Physics.Linecast(Main.MainCamera.transform.position, AimboneVec, out raycastHit, vis_mask) && raycastHit.collider && raycastHit.collider.gameObject.transform.root.gameObject == player.gameObject.transform.root.gameObject; //player.PlayerBones.Head.position
        }

        public static Vector3 GetShootPos()
        {
            if (Main.LocalPlayer == null)
            {
                return Vector3.zero;
            }
            Player.FirearmController firearmController = Main.LocalPlayer.HandsController as Player.FirearmController;
            if (firearmController == null)
            {
                return Vector3.zero;
            }
            return firearmController.Fireport.position;
        }


        /*
        public void hkInitiateShot(GClass1525 ammo, Vector3 shotPosition, Vector3 shotDirection)
        {
            if (Settings.Aimbot && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.MainCamera != null)
            {
                GamePlayer _target = GetTarget(Aimbot._targetList);
                if (_target != null)
                {
                    //Vector3 start = Main.LocalPlayer.PlayerBones.Fireport.position;
                    //Vector3 end = _target.Player.PlayerBones.Head.Original.transform.position;
                    //Vector3 dir = end - start;
                    //shotPosition = start;
                    //shotDirection = dir;

                    shotPosition = Main.LocalPlayer.PlayerBones.Fireport.position;
                    shotDirection = _target.Player.PlayerBones.Head.Original.transform.position - Main.LocalPlayer.PlayerBones.Fireport.position;
                }
            }
            b1ghook.SilentAimbothk.Unhook();
            b1ghook.SilentAimbothk.OriginalMethod.Invoke(this, new object[] { ammo, shotPosition, shotDirection });
            b1ghook.SilentAimbothk.Hook();
        }
        */
        /*
        public void aimbotPointCircle()
        {
            if (this.end != Vector3.zero && GameScene.IsLoaded() && GameScene.InMatch() && Main.LocalPlayer != null && Main.LocalPlayer.Weapon != null && Main.MainCamera != null)
            {
                //Render.Circle(Main.MainCamera.WorldToScreenPoint(this._aimTarget).x - 5f, Main.MainCamera.WorldToScreenPoint(this._aimTarget).y - 5f, 10f);
                Circle.DrawCircle(new Vector2(Main.MainCamera.WorldToScreenPoint(this.end).x, Main.MainCamera.WorldToScreenPoint(this.end).y), 8, Color.yellow, 2, 8);
            }
        }
        */
        /*private void OnGUI()
        {

            if (Settings.DrawAimbotPoint)
            {
                aimbotPointCircle();
            }

        }*/
    }

}
