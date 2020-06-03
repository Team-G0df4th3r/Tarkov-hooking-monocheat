using EFT.Interactive;
using System;
using UnityEngine;

namespace MonoSecurity
{
    internal class GameLootContainer
    {

        public LootableContainer LootableContainer { get; }

        private Vector3 screenPosition;

        public Vector3 ScreenPosition
        {
            get
            {
                return this.screenPosition;
            }
        }


        public bool IsOnScreen { get; private set; }


        public float Distance { get; private set; }


        public string FormattedDistance
        {
            get
            {
                return string.Format("{0}m", Math.Round(this.Distance));
            }
        }

        public GameLootContainer(LootableContainer lootableContainer)
        {
            this.LootableContainer = lootableContainer ?? throw new ArgumentNullException("lootableContainer");
            this.screenPosition = default;
            this.Distance = 0f;
        }

        public void RecalculateDynamics()
        {
            if (!GameUtils.IsLootableContainerValid(this.LootableContainer))
            {
                return;
            }
            this.screenPosition = GameUtils.WorldPointToScreenPoint(this.LootableContainer.transform.position);
            this.IsOnScreen = GameUtils.IsScreenPointVisible(this.screenPosition);
            this.Distance = Vector3.Distance(Main.MainCamera.transform.position, this.LootableContainer.transform.position);
        }

    }
}
