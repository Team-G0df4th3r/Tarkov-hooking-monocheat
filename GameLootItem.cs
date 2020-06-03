using EFT.Interactive;
using System;
using UnityEngine;

namespace MonoSecurity
{
    public class GameLootItem
    {

        private Vector3 screenPosition;

        public LootItem LootItem { get; }

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
                return string.Format("{0}m", Math.Round((double)this.Distance));
            }
        }

        public GameLootItem(LootItem lootItem)
        {
            this.LootItem = lootItem ?? throw new ArgumentNullException("lootItem");
            this.screenPosition = default;
            this.Distance = 0f;
        }

        public void RecalculateDynamics()
        {
            if (!GameUtils.IsLootItemValid(this.LootItem))
            {
                return;
            }
            this.screenPosition = GameUtils.WorldPointToScreenPoint(this.LootItem.transform.position);
            this.IsOnScreen = GameUtils.IsScreenPointVisible(this.screenPosition);
            this.Distance = Vector3.Distance(Main.MainCamera.transform.position, this.LootItem.transform.position);
        }


    }
}
