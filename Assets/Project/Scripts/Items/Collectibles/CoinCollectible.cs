using LS.Events;
using UnityEngine;

namespace LS.Items.Collectibles
{
    public class CoinCollectible : BaseCollectible
    {
        public override int ResourceID => 0;
        public override int Value => 1;

        public override void OnCollected()
        {
            base.OnCollected();
            GameEvents.OnCoinsCollected?.Invoke(Value);
        }
    }
}
