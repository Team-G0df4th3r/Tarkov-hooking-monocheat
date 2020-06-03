using EFT;
using EFT.Animations;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MonoSecurity
{
    public class B1ghook
    {
        public object SilentaimHk(object ammo, Vector3 origin, Vector3 direction, int fireIndex, Player player, Item weapon, float speedFactor = 1f, int fragmentIndex = 0)
        {
            if (Settings.SilentAim && Main.LocalPlayer != null && Main.LocalPlayer.HandsController != null && Main.LocalPlayer.HandsController.Item != null && Main.LocalPlayer.HandsController.Item.GetItemComponent<KnifeComponent>() == null)
            {
                IEnumerable<GamePlayer> _targetList;
                HashSet<Player> PlayerList = new HashSet<Player>();

                _targetList = from p in Main.GamePlayers
                              where !p.Player.IsYourPlayer() && GameUtils.IsPlayerAlive(p.Player) && !GameUtils.IsInYourGroup(p.Player)
                              select p;
                _targetList = from p in _targetList
                              orderby p.Fov, p.Distance
                              select p;
                foreach (GamePlayer gamePlayer in _targetList)
                {
                    if (gamePlayer.Distance <= (float)Settings.AimBotDistance && gamePlayer.Fov <= Settings.AimbotFOV)
                    {
                        PlayerList.Add(gamePlayer.Player);
                    }
                }

                if (PlayerList.Count > 0)
                {
                    Player Target = PlayerList.First();
                    Weapon LocalWeapon = Main.LocalPlayer.Weapon;
                    int boneID = GameUtils.AimBoneTarget();
                    if (LocalWeapon != null)
                    {
                        Vector3 TargetVec = GameUtils.GetBonePosByID(Target, boneID);
                        direction = (TargetVec - origin).normalized;
                    }
                }
            }
            Misc.create_sa_hook.Unhook();
            object[] parameters = new object[]
            {
                ammo,
                origin,
                direction,
                fireIndex,
                player,
                weapon,
                speedFactor,
                fragmentIndex
            };
            object result = Misc.create_sa_hook.OriginalMethod.Invoke(this, parameters);
            Misc.create_sa_hook.Hook();
            return result;
        }


        public bool PenHk(object shot, Vector3 hitpoint)
        {
            return true;
        }

        public bool DefHk(float _hitCosDirectionToNormal, object shot, Vector3 hitpoint, Vector3 shotNormal, Vector3 shotDirection)
        {
            return false;
        }

        public bool DoorHk(Vector3 yourPosition)
        {
            return true;
        }

        public bool InfHk()
        {
            return true;
        }

        public bool GammaHk(String equipmentSlotID)
        {
            return true;
        }

        public bool KeyHk()
        {
            return false;
        }

        public bool BindHk(Item item)
        {
            return true;
        }

        public bool SlotHk(Slot slot, EquipmentSlot slotName)
        {
            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void PainHk(float damage)
        {
            return;
        }

        public bool CertHk(byte[] certificateData)
        {
            return true;
        }

        public void FlyHk(ref Vector3 motion, float deltaTime, bool stickToGround)
        {
            stickToGround = false;
            Misc.create_exam_hook.Unhook();
            object[] parameters = new object[]
            {
                motion,
                deltaTime,
                stickToGround
            };
            Misc.create_exam_hook.OriginalMethod.Invoke(this, parameters);
            Misc.create_exam_hook.Hook();
        }

        public Vector2 StationaryHk()
        {
            Vector2 vector = new Vector2(180f, 180f);
            return vector;
        }

        public ENetLogsLevel LogHk()
        {
            return ENetLogsLevel.None;
        }

        public void PenStatHk(object shot, float penetrationPower = 9999f)
        {

            penetrationPower = 9999f;

            Misc.create_sa_hook.Unhook();
            object[] parameters = new object[]
            {
                shot,
                penetrationPower
            };
            Misc.create_sa_hook.OriginalMethod.Invoke(this, parameters);
            Misc.create_sa_hook.Hook();

        }

        public float PitchHk()
        {
            FieldInfo Hinge = typeof(StationaryWeapon).GetField("Hinge");
            Transform HingeT = (Transform)Hinge.GetValue(this);

            float num = HingeT.rotation.eulerAngles.x;

            // rotation.eulerAngles.x + 90f;
            if (num > 180f)
            {
                return num - 360f;
            }
            if (num >= -180f)
            {
                return num;
            }
            return num + 360f;
        }

        public void OnRotationHk()
        {
            Type AGSMachinery = typeof(AGSMachinery);

            FieldInfo transform_0_f = AGSMachinery.GetField("transform_0");
            FieldInfo transform_1_f = AGSMachinery.GetField("transform_1");
            FieldInfo transform_2_f = AGSMachinery.GetField("transform_2");
            FieldInfo transform_3_f = AGSMachinery.GetField("transform_3");

            Transform transform_0 = (Transform)transform_0_f.GetValue(this);
            Transform transform_1 = (Transform)transform_1_f.GetValue(this);
            Transform transform_2 = (Transform)transform_2_f.GetValue(this);
            Transform transform_3 = (Transform)transform_3_f.GetValue(this);

            Vector3 position = transform_1.position;
            Vector3 position2 = transform_0.position;
            Vector3 localPosition = transform_2.localPosition;
            float num = Vector3.Distance(position, position2);
            float num2 = Mathf.Asin(localPosition.y / num);
            transform_2.localPosition = new Vector3(localPosition.x, localPosition.y, num);
            Vector3 vector = position2 - position;
            Vector3 vector2 = transform_1.parent.InverseTransformDirection(vector.normalized);
            transform_1.localRotation = Quaternion.Euler(180f, 0f, 0f); //(num2 - Mathf.Asin(vector2.y)) * 57.29578f
            transform_3.rotation = Quaternion.LookRotation(Vector3.up, transform_3.up);
        }

        public bool ExamineHk(object item)
        {
            return true;
        }

        public float SpdHk()
        {
            return 10f;
        }

        public float SpdHk2()
        {
            return 10f;
        }

        public bool StandHk(float h)
        {
            return true;
        }

        public ENicknameError NmeHk(string textValue)
        {
            return ENicknameError.ValidNickname;
        }

        public void WeightHk()
        {
            Main.LocalPlayer.Physical.WalkOverweightLimits.Set(1f, 10000f);
            Main.LocalPlayer.Physical.BaseOverweightLimits.Set(1f, 10000f);
            Main.LocalPlayer.Physical.SprintOverweightLimits.Set(1f, 10000f);
            Main.LocalPlayer.Physical.WalkSpeedOverweightLimits.Set(1f, 10000f);
        }

        public float MeleeHk()
        {
            return 10f;
        }

        public bool GroundHk(float depth, Vector3? axis = null, float width = 0f, float heightDivider = 4f, float extraCastLn = 0f)
        {
            return false;
        }


        /*
		public void grenadeHk(object grenade, object grenadeItem, Vector3 grenadePosition, Player playerWhoThrew, object grenadeBallisticsCalculator, Item originalWeaponItem)
		{
			if (Settings.GrenadeAimbot)
			{
				IEnumerable<GamePlayer> _targetList;
				HashSet<Player> PlayerList = new HashSet<Player>();

				_targetList = from p in Main.GamePlayers
							  where !p.Player.IsYourPlayer() && GameUtils.IsPlayerAlive(p.Player) && !GameUtils.IsInYourGroup(p.Player)
							  select p;
				_targetList = from p in _targetList
							  orderby p.Fov, p.Distance
							  select p;
				foreach (GamePlayer gamePlayer in _targetList)
				{
					if (gamePlayer.Distance <= (float)Settings.AimBotDistance && gamePlayer.Fov <= Settings.AimbotFOV && gamePlayer.IsOnScreen)
					{
						PlayerList.Add(gamePlayer.Player);
					}
				}

				if (PlayerList.Count > 0)
				{
					Player Target = PlayerList.First<Player>();
					Weapon LocalWeapon = Main.LocalPlayer.Weapon;
					if (LocalWeapon != null)
					{
						grenadePosition = Target.PlayerBones.Head.position.normalized;
						
					}
				}
			}
			Misc.create_ga_hook.Unhook();
			object[] parameters = new object[]
			{
				grenade,
				grenadeItem,
				grenadePosition,
				playerWhoThrew,
				grenadeBallisticsCalculator,
				originalWeaponItem
			};
			//object result = Misc.create_ga_hook.OriginalMethod.Invoke(this, parameters);
			Misc.create_ga_hook.Hook();
			//return result;
		}
		*/

        /*
		public void UpdateWeightLimitshk()
		{
			if (Main.LocalPlayer != null && Settings.Removeweight)
			{
				Main.LocalPlayer.Physical.WalkOverweightLimits.Set(1f, 10000f);
				Main.LocalPlayer.Physical.BaseOverweightLimits.Set(1f, 10000f);
				Main.LocalPlayer.Physical.SprintOverweightLimits.Set(1f, 10000f);
				Main.LocalPlayer.Physical.WalkSpeedOverweightLimits.Set(1f, 10000f);
			}
		}
		*/
    }
}
