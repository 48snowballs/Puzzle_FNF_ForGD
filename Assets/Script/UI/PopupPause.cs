﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPause : UIPanel
{
    public override UI_PANEL GetID()
    {
        return UI_PANEL.PopupPause;
    }

    public static void Show()
    {
        PopupPause newInstance = (PopupPause)GUIManager.Instance.NewPanel(UI_PANEL.PopupPause);
        newInstance.OnAppear();
    }
    public void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();
    }
    public void ButtonResume()
    {
        AudioManager.Instance.ResumeGame();
        Time.timeScale = 1;
        Close();
        EvenGlobalManager.Instance.OnActiveTarget.Dispatch(true);
    }
    public void ButtonRestart()
    {
        AudioManager.Instance.StopGame();
        Time.timeScale = 1;
        Close();
        EvenGlobalManager.Instance.OnEndPlay.Dispatch(false);
        EvenGlobalManager.Instance.OnStartPlay.Dispatch();
        PlayScreen.Show();
    }
    public void ButtonMainMenu()
    {
        Time.timeScale = 1;
        Close();
        EvenGlobalManager.Instance.OnEndPlay.Dispatch(false);
        MainScreen.Show();
    }
}
