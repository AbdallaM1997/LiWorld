using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class StoreManager : MonoBehaviour
{
    public CoinSystem coinSystem;

    public void OnPurchasingCompleted(Product product)
    {
        if (product != null)
        {
            switch (product.definition.id)
            {
                case "coin.25":
                    coinSystem.BuyCoins(25);
                    break;
                case "coin.125":
                    coinSystem.BuyCoins(125);
                    break;
                case "coin.250":
                    coinSystem.BuyCoins(250);
                    break;
                case "coin.500":
                    coinSystem.BuyCoins(500);
                    break;
                case "coin.1000":
                    coinSystem.BuyCoins(1000);
                    break;
                default:
                    coinSystem.Massage("Not Have enough Money");
                    break;
            }
        }

    }
}
