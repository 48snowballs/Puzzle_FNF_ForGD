using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainScreen : UIPanel
{
    public TMP_Text coinText;
    public override UI_PANEL GetID()
    {
        return UI_PANEL.MainScreen;
    }

    public static MainScreen Instance;

    public static void Show()
    {
        MainScreen newInstance = (MainScreen)GUIManager.Instance.NewPanel(UI_PANEL.MainScreen);
        Instance = newInstance;
        newInstance.OnAppear();
    }
    public void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();
        coinText.text = GameManager.Instance.Data.Gold.ToString();
    }
    public void ButtonPlay()
    {
        PlayScreen.Show();
        EvenGlobalManager.Instance.OnStartPlay.Dispatch();
    }
}
