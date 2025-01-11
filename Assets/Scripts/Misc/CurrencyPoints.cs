using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyPoints : Singleton<CurrencyPoints>
{
    private TMP_Text goldText;
    private static int currentGold = 0; // Static variable persists across scenes

    const string COIN_AMOUNT_TEXT = "Coin Amount Text";

    public void UpdateCurrentGold()
    {
        currentGold += 1;

        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }
}

