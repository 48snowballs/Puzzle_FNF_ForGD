using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreen : UIPanel
{
    public TMP_Text coinText;
    public TMP_Text goldText;

    private int gold;
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
        coinText.text = GameManager.Instance.Data.Gold.ToString();
        this.gold = gold;
        goldText.text = gold.ToString();
    }
    public void ButtonMainMenu()
    {
        GameManager.Instance.Data.Level++;
        GameManager.Instance.Data.Gold += gold;
        Database.SaveData();
        MainScreen.Show();
    }
}
