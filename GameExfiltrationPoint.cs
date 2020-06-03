using EFT.Interactive;
using System;
using UnityEngine;

namespace MonoSecurity
{
    internal class GameExfiltrationPoint
    {
        private Vector3 screenPosition;


        public ExfiltrationPoint ExfiltrationPoint { get; }


        public Vector3 ScreenPosition
        {
            get
            {
                return this.screenPosition;
            }
        }


        public bool IsOnScreen { get; private set; }


        public float Distance { get; private set; }


        public string Name { get; set; }

        public string FormattedDistance
        {
            get
            {
                return string.Format("{0}m", Math.Round((double)this.Distance));
            }
        }

        public GameExfiltrationPoint(ExfiltrationPoint exfiltrationPoint)
        {
            this.ExfiltrationPoint = exfiltrationPoint ?? throw new ArgumentNullException("exfiltrationPoint");
            this.screenPosition = default;
            this.Distance = 0f;
            this.Name = exfiltrationPoint.name.Localized();
        }

        public void RecalculateDynamics()
        {
            if (!GameUtils.IsExfiltrationPointValid(this.ExfiltrationPoint))
            {
                return;
            }
            this.screenPosition = GameUtils.WorldPointToScreenPoint(this.ExfiltrationPoint.transform.position);
            this.IsOnScreen = GameUtils.IsScreenPointVisible(this.screenPosition);
            this.Distance = Vector3.Distance(Main.MainCamera.transform.position, this.ExfiltrationPoint.transform.position);
        }

    }
}
