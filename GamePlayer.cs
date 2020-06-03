using EFT;
using System;
using UnityEngine;

namespace MonoSecurity
{
    public class GamePlayer
    {
        public static string Group = string.Empty;

        private Vector3 screenPosition;

        private Vector3 headScreenPosition;

        private Color _playerColor = new Color(32, 186, 173);

        private Color _deadPlayerColor = new Color(105, 105, 105);

        private Color _botColor = new Color(255, 255, 255);

        private Color _bossColor = new Color(247, 62, 210);

        private Color _raiderColor = new Color(143, 0, 254);

        private Color _friendColor = new Color(40, 250, 89);

        public enum BodyPart
        {
            Pelvis = 14,
            LThigh1,
            LThigh2,
            LCalf,
            LFoot,
            LToe,
            RThigh1,
            RThigh2,
            RCalf,
            RFoot,
            RToe,
            Bear_Feet,
            USEC_Feet,
            BEAR_feet_1,
            Spine1 = 29,
            Gear1,
            Gear2,
            Gear3,
            Gear4,
            Gear4_1,
            Gear5,
            Spine2,
            Spine3,
            Ribcage = 66,
            LCollarbone = 89,
            LUpperarm,
            LForearm1,
            LForearm2,
            LForearm3,
            LPalm,
            RUpperarm = 111,
            RForearm1,
            RForearm2,
            RForearm3,
            RPalm,
            Neck = 132,
            Head
        }

        public enum PlayerType
        {
            Scav,
            PlayerScav,
            Player,
            Friend,
            ScavGuard,
            Boss,
            Raider,
            Dead
        }

        public Player Player { get; }

        public Vector3 ScreenPosition
        {
            get
            {
                return this.screenPosition;
            }
        }

        public Vector3 HeadScreenPosition
        {
            get
            {
                return this.headScreenPosition;
            }
        }

        public bool IsOnScreen { get; private set; }


        public bool IsVisible { get; private set; }



        public float Fov { get; set; }

        public float Distance { get; private set; }


        public bool IsAI { get; private set; }



        public string FormattedDistance
        {
            get
            {
                return string.Format("{0}m", Math.Round((double)this.Distance));
            }
        }

        public GamePlayer(Player player)
        {
            this.Player = player ?? throw new ArgumentNullException("player");
            this.screenPosition = default;
            this.headScreenPosition = default;
            this.IsOnScreen = false;
            this.Distance = 0f;
            this.IsAI = true;
            this.Fov = 0f;

        }

        public void RecalculateDynamics()
        {
            if (!GameUtils.IsPlayerValid(this.Player))
            {
                return;
            }
            this.screenPosition = GameUtils.WorldPointToScreenPoint(this.Player.Transform.position);
            if (this.Player.PlayerBones != null)
            {
                this.headScreenPosition = GameUtils.WorldPointToScreenPoint(this.Player.PlayerBones.Head.position);
            }
            this.IsOnScreen = GameUtils.IsScreenPointVisible(this.screenPosition);
            this.Distance = Vector3.Distance(Main.MainCamera.transform.position, this.Player.Transform.position);
            this.IsVisible = this.IsVisibles();
            this.Fov = this.GetFov();
            if (this.Player.Profile != null && this.Player.Profile.Info != null)
            {
                this.IsAI = (this.Player.Profile.Info.RegistrationDate <= 0);
            }

        }


        private bool IsVisibles()
        {
            RaycastHit raycastHit;
            return this.IsOnScreen && (Physics.Linecast(Main.MainCamera.transform.position, GameUtils.GetBonePosByID(this.Player, 133), out raycastHit) && raycastHit.transform.gameObject == this.Player.gameObject);
        }

        public float GetFov()
        {
            Vector3 position = Main.MainCamera.transform.position;
            Vector3 forward = Main.MainCamera.transform.forward;
            Vector3 normalized = (GameUtils.GetBonePosByID(this.Player, Settings.AimBotBone) - position).normalized;
            return (Mathf.Acos(Mathf.Clamp(Vector3.Dot(forward, normalized), -1f, 1f)) * 57.29578f) * 21f; // / 21f;
        }

    }
}
