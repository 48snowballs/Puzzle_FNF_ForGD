using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinScreen : UIPanel
{
    public TMP_Text coinText;
    public TMP_Text goldText;
    public Button adBtn;

    private int gold;
    private bool isAdGold;
    public override UI_PANEL GetID()
    {
        return UI_PANEL.WinScreen;
    }

    public static WinScreen Instance;

    public static void Show(int gold)
    {
        WinScreen newInstance = (WinScreen)GUIManager.Instance.NewPanel(UI_PANEL.WinScreen);
        Instance = newInstance;
        newInstance.OnAppear(gold);
    }
    public void OnAppear(int gold)
    {
        if (isInited)
            return;

        base.OnAppear();

        isAdGold = false;
        coinText.text = GameManager.Instance.Data.Gold.ToString();
        this.gold = gold;
        goldText.text = gold.ToString();
    }
    public void ButtonMainMenu()
    {
        if (!isAdGold)
        {
            Utils.ChangeText(goldText, gold, 0, 1);
            Utils.ChangeText(coinText, GameManager.Instance.Data.Gold, GameManager.Instance.Data.Gold + gold, 1);

            GameManager.Instance.Data.Level++;
            GameManager.Instance.Data.Gold += gold;
            Database.SaveData();

            Invoke(nameof(GotoMainScreen), 2f);
        } else
        {
            MainScreen.Show();
        }      
    }
    public void GotoMainScreen()
    {
        MainScreen.Show();
    }
    public void ButtonBonusCoin()
    {
        isAdGold = true;
        adBtn.gameObject.SetActive(false);

        gold *= 5;
        Utils.ChangeText(goldText, gold, 0, 1);
        Utils.ChangeText(coinText, GameManager.Instance.Data.Gold, GameManager.Instance.Data.Gold + gold, 1);

        GameManager.Instance.Data.Gold += gold;
        Database.SaveData();
    }
}
