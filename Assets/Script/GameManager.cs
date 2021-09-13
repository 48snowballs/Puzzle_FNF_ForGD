using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameData Data;
    public SongManager songManager;

    public SkeletonDataAsset[] character;
    public Sprite[] characterImg;
    // Start is called before the first frame update
    void Start()
    {
        Data = Database.LoadData();
        if (Data == null)
        {
            Data = new GameData();
            //for (int i = 0; i < songManager.songAudio.Count; i++)
            //{
            //    Data.ListUnlockSong.Add(false);
            //    Data.ListScoreStoryEasy.Add(0);
            //    Data.ListScoreStoryNormal.Add(0);
            //    Data.ListScoreStoryHard.Add(0);
            //    Data.ListScoreFreeEasy.Add(0);
            //    Data.ListScoreFreeNormal.Add(0);
            //    Data.ListScoreFreeHard.Add(0);
            //}
            //Data.ListUnlockSong[0] = true;
            Database.SaveData();
        }
        SplashManager.Instance.Load();
        Application.targetFrameRate = 60;
    }
}
