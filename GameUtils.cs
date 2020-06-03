using Diz.Skinning;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using MonoSecurity.Drawing;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MonoSecurity
{
    public static class GameUtils
    {
        public static float Map(float value, float sourceFrom, float sourceTo, float destinationFrom, float destinationTo)
        {
            return (value - sourceFrom) / (sourceTo - sourceFrom) * (destinationTo - destinationFrom) + destinationFrom;
        }

        public static bool IsPlayerValid(Player player)
        {
            return player != null && player.Transform != null && player.PlayerBones != null && player.PlayerBones.transform != null;
        }

        public static bool IsExfiltrationPointValid(ExfiltrationPoint exfiltrationPoint)
        {
            return exfiltrationPoint != null;
        }

        public static bool IsLootItemValid(LootItem lootItem)
        {
            return lootItem != null && lootItem.Item != null && lootItem.Item.Template != null;
        }

        public static bool IsCorpseValid(EFT.Interactive.Corpse corpse)
        {
            return corpse != null;
        }

        public static bool IsLootableContainerValid(LootableContainer lootableContainer)
        {
            return lootableContainer != null && lootableContainer.Template != null;
        }

        public static bool IsThrowableValid(Throwable throwable)
        {
            return throwable != null;
        }

        public static bool IsPlayerAlive(Player player)
        {
            return GameUtils.IsPlayerValid(player) && player.HealthController != null && player.HealthController.IsAlive;
        }

        public static int AimBoneTarget()
        {
            if (Settings.BoneChange == 1)
            {
                return 133;
            }
            else if (Settings.BoneChange == 2)
            {
                return 66;
            }
            else if (Settings.BoneChange == 3)
            {
                return 22;
            }
            else
            {
                return 133;
            }
        }

        public static string BoneName(int bone)
        {
            switch (bone)
            {
                case 133:
                    return "Head";
                case 66:
                    return "Ribcage";
                case 22:
                    return "Right Calf";
                default:
                    return "Unknown";

            }
        }

        public static Vector3 WorldPointToScreenPoint(Vector3 worldPoint)
        {
            Vector3 vector = Main.MainCamera.WorldToScreenPoint(worldPoint);
            vector.y = (float)Screen.height - vector.y;
            return vector;
        }

        public static bool IsScreenPointVisible(Vector3 screenPoint)
        {
            return screenPoint.z > 0.01f && screenPoint.x > -5f && screenPoint.y > -5f && screenPoint.x < (float)Screen.width && screenPoint.y < (float)Screen.height;
        }

        public static Vector3 GetBonePosByID(Player player, int id)
        {
            Vector3 result;
            try
            {
                result = GameUtils.SkeletonBonePos(player.PlayerBones.AnimatedTransform.Original.gameObject.GetComponent<PlayerBody>().SkeletonRootJoint, id);
            }
            catch (Exception)
            {
                result = Vector3.zero;
            }
            return result;
        }

        public static Vector3 SkeletonBonePos(Skeleton skeleton, int id)
        {
            return skeleton.Bones.ElementAt(id).Value.position;
        }

        public static bool IsInYourGroup(Player player)
        {
            string Group = Main.LocalPlayer.Profile.Info.GroupId;
            return Group == player.Profile.Info.GroupId && Group != "0" && Group != "" && Group != null;
        }

        public static Type FindType(string typeName)
        {

            return typeof(EFT.Player).Assembly.GetType(typeName, false, true);

        }

        public static MethodInfo FindMethod(string typeName, string methodName)
        {

            return FindType(typeName).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

        }

        public static FieldInfo GetField(int token)
        {
            return typeof(EFT.Player).Assembly.GetType().Module.ResolveField(token);
        }

        public static Type GetType(int token)
        {
            return typeof(EFT.Player).Assembly.GetType().Module.ResolveType(token);
        }

        public static FieldInfo FindSecret<T>(this T classType, string fieldName)
        {
            FieldInfo field = typeof(T).GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field;
        }
        public static FieldInfo FindFieldInfo<T>(string fieldName)
        {
            return typeof(T).GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        }
        public static void SetSecret<T>(this FieldInfo target, T value, object targetClass = null)
        {
            target.SetValue(targetClass, (object)value);
        }
        public static T GetSecret<T>(this FieldInfo target, object targetClass = null)
        {
            return (T)target.GetValue(targetClass);
        }
        public static T GetSecret<T>(this object target, string fieldName)
        {
            try
            {
                Type objectType = target.GetType();
                FieldInfo fieldInfo = objectType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreCase);

                return (T)fieldInfo.GetValue(target);
            }
            catch
            {

            }
            return default(T);
        }
        public static void SetSecret<T>(this object target, string fieldName, T value)
        {
            target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).SetValue(target, value);
        }
        public static void SetSecretProperty(this object target, string propertyName, object[] value)
        {
            target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetSetMethod(false).Invoke(target, value);
        }
        public static T GetSecretProperty<T>(this object target, string propertyName)
        {
            return (T)target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetGetMethod(false).Invoke(target, null);
        }
        public static retType GetFieldValueToken<retType>(this object classObject, int token)
        {
            return (retType)(classObject.GetType().Module.ResolveField(token).GetValue(classObject));
        }

        public static void DrawSkeleton(GamePlayer gamePlayer, float thickness, Color color)
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

            Render.DrawLine(new Vector2(Neck.x, (float)Screen.height - Neck.y), new Vector2(Pelvis.x, (float)Screen.height - Pelvis.y), thickness, color);
            Render.DrawLine(new Vector2(LeftShoulder.x, (float)Screen.height - LeftShoulder.y), new Vector2(LeftElbow.x, (float)Screen.height - LeftElbow.y), thickness, color);
            Render.DrawLine(new Vector2(RightShoulder.x, (float)Screen.height - RightShoulder.y), new Vector2(RightElbow.x, (float)Screen.height - RightElbow.y), thickness, color);
            Render.DrawLine(new Vector2(LeftElbow.x, (float)Screen.height - LeftElbow.y), new Vector2(LeftPalm.x, (float)Screen.height - LeftPalm.y), thickness, color);
            Render.DrawLine(new Vector2(RightElbow.x, (float)Screen.height - RightElbow.y), new Vector2(RightPalm.x, (float)Screen.height - RightPalm.y), thickness, color);
            Render.DrawLine(new Vector2(RightShoulder.x, (float)Screen.height - RightShoulder.y), new Vector2(LeftShoulder.x, (float)Screen.height - LeftShoulder.y), thickness, color);
            Render.DrawLine(new Vector2(LeftKnee.x, (float)Screen.height - LeftKnee.y), new Vector2(Pelvis.x, (float)Screen.height - Pelvis.y), thickness, color);
            Render.DrawLine(new Vector2(RightKnee.x, (float)Screen.height - RightKnee.y), new Vector2(Pelvis.x, (float)Screen.height - Pelvis.y), thickness, color);
            Render.DrawLine(new Vector2(LeftKnee.x, (float)Screen.height - LeftKnee.y), new Vector2(Leftfoot.x, (float)Screen.height - Leftfoot.y), thickness, color);
            Render.DrawLine(new Vector2(RightKnee.x, (float)Screen.height - RightKnee.y), new Vector2(KickingFoot.x, (float)Screen.height - KickingFoot.y), thickness, color);
            Render.DrawLine(new Vector2(Neck.x, (float)Screen.height - Neck.y), new Vector2(head.x, (float)Screen.height - head.y), thickness, color);
            Circle.DrawCircleMain(new Vector2(head.x, (float)Screen.height - head.y), radius, color, thickness, false, 6);
        }

        public static int PlayerValue(GamePlayer gamePlayer)
        {
            int credit = 0;
            foreach (Item item in gamePlayer.Player.Profile.Inventory.Equipment.GetAllItems())
            {
                if (item.Template._parent != "5448bf274bdc2dfc2f8b456a")
                    credit += item.Template.CreditsPrice;
            }
            return credit;
        }

        public static void DrawHeadLine(GamePlayer gamePlayer)
        {
            //Vector3 WeaponPos = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.WeaponRoot.position);
            Vector3 HeadPos = Main.MainCamera.WorldToScreenPoint(GameUtils.GetBonePosByID(gamePlayer.Player, 133));
            Vector3 UpAndForward = (gamePlayer.Player.PlayerBones.WeaponRoot.up) * 6f;
            Vector3 WeaponPosForward = Main.MainCamera.WorldToScreenPoint(gamePlayer.Player.PlayerBones.WeaponRoot.position - UpAndForward);
            Render.DrawLine(new Vector2(WeaponPosForward.x, (float)Screen.height - WeaponPosForward.y), new Vector2(HeadPos.x, (float)Screen.height - HeadPos.y), 1.5f, Color.red);
        }
    }
}
