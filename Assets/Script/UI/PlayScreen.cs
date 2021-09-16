using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using TMPro;
using DG.Tweening;
public class PlayScreen : UIPanel
{
    public CountdownTimer countdownTimer;
    public TMP_Text watchArrowTxt;
    public TMP_Text repeatTxt;
    public SkeletonGraphic skePlayer;
    public SkeletonGraphic skeEnemy1;
    public SkeletonGraphic skeEnemy2;
    public List<GameObject> listHeart;
    public Image progressImg;

    private Level level;
    private bool isUseEnemy1;
    private int idxOfHeart;
    private bool isLose;

    public override UI_PANEL GetID()
    {
        return UI_PANEL.PlayScreen;
    }

    public static PlayScreen Instance;

    public static void Show(bool isReload = true)
    {
        PlayScreen newInstance = (PlayScreen)GUIManager.Instance.NewPanel(UI_PANEL.PlayScreen);
        Instance = newInstance;
        newInstance.OnAppear(isReload);
    }
    public void OnAppear(bool isReload = true)
    {
        if (isInited)
            return;

        base.OnAppear();

        Init(isReload);
    }
    void Init(bool isReload = true)
    { 
        level = GameManager.Instance.songManager.GetLevelData(GameManager.Instance.Data.Level%8);
        if (level.avatar == 2)
        {
            isUseEnemy1 = true;
            skeEnemy2.gameObject.SetActive(false);
            skeEnemy1.gameObject.SetActive(true);
            skeEnemy1.Clear();
            skeEnemy1.skeletonDataAsset = GameManager.Instance.character[level.avatar];
            skeEnemy1.Initialize(true);
            skeEnemy1.AnimationState.SetAnimation(0, "idle", true);
        } else if (level.avatar == 3 || level.avatar == 4)
        {
            isUseEnemy1 = false;
            skeEnemy1.gameObject.SetActive(false);
            skeEnemy2.gameObject.SetActive(true);
            skeEnemy2.Clear();
            skeEnemy2.skeletonDataAsset = GameManager.Instance.character[level.avatar];
            skeEnemy2.Initialize(true);
            skeEnemy2.AnimationState.SetAnimation(0, "idle", true);
        }
        if (isReload)
        {
            progressImg.fillAmount = 0;
        }

        idxOfHeart = 6;
        for (int i = 0; i < listHeart.Count; i++)
            listHeart[i].SetActive(true);

        isLose = false;
        CloseCountDown();
        repeatTxt.gameObject.SetActive(false);
    }
    public void ShowCountDown(float s)
    {
        countdownTimer.StartCountDown(s);
    }
    public void CloseCountDown()
    {
        countdownTimer.StopCountDown();
    }
    public void SetActiveWatchArrowText(bool active)
    {
        if (active)
        {
            watchArrowTxt.gameObject.SetActive(true);
            watchArrowTxt.DOPause();
            watchArrowTxt.color = new Color(1, 1, 1, 0);
            watchArrowTxt.DOColor(new Color(1, 1, 1, 1), 0.5f).SetDelay(1f);
        }
        else
        {
            watchArrowTxt.DOPause();
            watchArrowTxt.DOColor(new Color(1, 1, 1, 0), 0.2f);
        }
    }
    public void SetActiveRepeatText(bool active)
    {
        if (active)
        {
            repeatTxt.gameObject.SetActive(true);
            repeatTxt.DOPause();
            repeatTxt.color = new Color(1, 1, 1, 0);
            repeatTxt.DOColor(new Color(1, 1, 1, 1), 0.5f);
        }
        else
        {
            repeatTxt.DOPause();
            repeatTxt.DOColor(new Color(1, 1, 1, 0), 0.2f);
        }
    }
    public void OnTapButtonDown(int anim)
    {
        string str = "iii";
        if (anim == 1)
            str = "ooo";
        else if (anim == 2)
            str = "aaa";
        else if (anim == 3)
            str = "eee";
        skePlayer.AnimationState.SetAnimation(0, str, false);
        skePlayer.AnimationState.AddAnimation(0, "idle", true, 0);
    }
    public void OnMiss()
    {
        skePlayer.AnimationState.SetAnimation(0, "sad", false);
        skePlayer.AnimationState.AddAnimation(0, "idle", true, 0);
    }
    public void OnPerfect()
    {
        skePlayer.AnimationState.SetAnimation(0, "happy", false);
        skePlayer.AnimationState.AddAnimation(0, "idle", true, 0);
    }
    public void IncorectAnswer()
    {
        idxOfHeart--;
        if (idxOfHeart >= 0)
        {
            listHeart[idxOfHeart].SetActive(false);
        }
        if (idxOfHeart <=0 && !isLose)
        {
            isLose = true;
            Invoke(nameof(ShowLoseScreen),1f);
            EvenGlobalManager.Instance.OnEndPlay.Dispatch(true);
        }
    }
    public void DoubleCorectAnswer()
    {
        if (idxOfHeart < 6)
        {
            listHeart[idxOfHeart].SetActive(true);
            idxOfHeart++;
        }
    }
    public void ShowLoseScreen()
    {
        LoseScreen.Show();
    }
    public void UpdateProgress(float end)
    {
        Utils.ChangeImageFill(progressImg, progressImg.fillAmount, end, 0.5f);
    }
    public void OnEnemyHit(int anim)
    {
        string str = "iii";
        if (anim == 1)
            str = "ooo";
        else if (anim == 2)
            str = "aaa";
        else if (anim == 3)
            str = "eee";
        //if (Context.selectedSong == 0 && Context.modePlay != MODE_PLAY.ENDLESS_MODE)
        //{
        //    skeEnemyTut.AnimationState.SetAnimation(0, str, false);
        //    skeEnemyTut.AnimationState.AddAnimation(0, "idle", true, 0);
        //}
        //else
        //{
            if (isUseEnemy1)
            {
                skeEnemy1.AnimationState.SetAnimation(0, str, false);
                skeEnemy1.AnimationState.AddAnimation(0, "idle", true, 0);
            }
            else
            {
                skeEnemy2.AnimationState.SetAnimation(0, str, false);
                skeEnemy2.AnimationState.AddAnimation(0, "idle", true, 0);
            }
   //     }
    }
    public void OnWin()
    {
        if (!isLose)
        {
            WinScreen.Show(level.gold);
        }
    }
    public void ButtonPause()
    {
        AudioManager.Instance.PauseGame();
        Time.timeScale = 0;
        PopupPause.Show();
        EvenGlobalManager.Instance.OnActiveTarget.Dispatch(false);
    }
}
