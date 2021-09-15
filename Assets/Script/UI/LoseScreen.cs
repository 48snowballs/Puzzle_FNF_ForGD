using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.LoseScreen;
    }

    public static LoseScreen Instance;

    public static void Show(bool isReload = true)
    {
        LoseScreen newInstance = (LoseScreen)GUIManager.Instance.NewPanel(UI_PANEL.LoseScreen);
        Instance = newInstance;
        newInstance.OnAppear(isReload);
    }
    public void OnAppear(bool isReload = true)
    {
        if (isInited)
            return;

        base.OnAppear();
    }
    public void ButtonMainMenu()
    {
        MainScreen.Show();
    }
    public void ButtonReplay()
    {
        PlayScreen.Show();
        AudioManager.Instance.StopGame();
        EvenGlobalManager.Instance.OnStartPlay.Dispatch();
    }
    public void ButtonContinue()
    {
        PlayScreen.Show(false);
        EvenGlobalManager.Instance.OnContinue.Dispatch();
    }
}
