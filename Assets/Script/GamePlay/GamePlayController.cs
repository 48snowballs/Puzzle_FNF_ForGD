using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    public Arrow prefab;
    public GameObject groupTap;
    public AnswerPool answerPool;
    public IncorectNotificaion incorect;
    public ObjectPerfect objStatePrefab;
    [SerializeField] private SkeletonAnimation[] lsButton;

    private bool enable;
    private bool blockInput;
    private int idxOfNode;
    private int idxOfArrow;
    private int idxOfAnswer;
    private STATE currentState;
    private int answerCount;
    private bool canSpawn;
    private ObjectPerfect textPerfect;
    private Vector2[] posStart = new Vector2[] { new Vector2(-2.1f, -3.5f), new Vector2(-0.7f, -3.5f), new Vector2(0.7f, -3.5f), new Vector2(2.1f, -3.5f) };

    private Level level;

    private List<Arrow> listArrow = new List<Arrow>();
    private bool isSpawn;
    private bool isOverTime = false;
    private int move;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GUIManager.Instance.ReloadCamera();
        MainScreen.Show();
    }

    private void OnEnable()
    {
        EvenGlobalManager.Instance.OnStartPlay.AddListener(OnStart);
        EvenGlobalManager.Instance.OnEndPlay.AddListener(OnEnd);
        EvenGlobalManager.Instance.OnContinue.AddListener(OnContinue);
        EvenGlobalManager.Instance.OnActiveTarget.AddListener(OnActiveTarget);
        EvenGlobalManager.Instance.OnArrowDisappear.AddListener(OnArrowDisappear);
    }
    public void OnStart()
    {
        level = GameManager.Instance.songManager.GetLevelData(GameManager.Instance.Data.Level%8);
        currentState = STATE.ASK_STATE;
        if (textPerfect == null)
            textPerfect = SimplePool.Spawn(objStatePrefab, new Vector3(0, 3), Quaternion.identity, false);
        textPerfect.gameObject.SetActive(false);
        for (int i = 0; i < lsButton.Length; i++)
            lsButton[i].AnimationState.SetAnimation(0, "idle", false);
        
        AudioManager.Instance.PlayGame(GameManager.Instance.songManager.GetSongClip(level.song).clipSong,0);

        isSpawn = false;
        enable = true;
        blockInput = false;
        canSpawn = true;
        idxOfNode = 0;
        PlayScreen.Instance.UpdateProgress(idxOfNode * 1f / level.numOfNodes);
        idxOfArrow = 0;
        idxOfAnswer = 0;
        answerCount = 0;
        listArrow = new List<Arrow>();
        move = 0;
    }
    public void OnContinue()
    {
        currentState = STATE.ASK_STATE;
        enable = true;
        blockInput = false;
        canSpawn = true;
        idxOfArrow = 0;
        idxOfAnswer = 0;
        answerCount = 0;
        listArrow = new List<Arrow>();
        move = 0;
        AudioManager.Instance.ResumeGame();
        OnActiveTarget(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (!blockInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnTouchDown(0);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                OnTouchDown(1);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                OnTouchDown(2);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                OnTouchDown(3);

            if (Input.GetKeyUp(KeyCode.LeftArrow))
                OnTouchUp();
            if (Input.GetKeyUp(KeyCode.DownArrow))
                OnTouchUp();
            if (Input.GetKeyUp(KeyCode.UpArrow))
                OnTouchUp();
            if (Input.GetKeyUp(KeyCode.RightArrow))
                OnTouchUp();

            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                    OnTouchDown(int.Parse(hit.collider.name));
                //MouseDown();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnTouchUp();
                //MouseUp();
            }
        }

        if (enable)
        {
            if (level.numOfNodes <= idxOfNode)
            {
                enable = false;
                blockInput = true;
                Invoke(nameof(OnWinGame),1f);
                return;
            }
            if (currentState == STATE.ASK_STATE)
            {
                if (!isSpawn)
                {
                    StartCoroutine(SpawnArrow());
                }
            } else if (currentState == STATE.ANSWER_STATE)
            {
                if (PlayScreen.Instance.countdownTimer.IsOverTime() && !isOverTime)
                {
                    StartCoroutine(OverTime());
                }
                if (answerCount>2)
                {
                    textPerfect.Setup(2);
                    PlayScreen.Instance.OnMiss();
                    NextNode();
                }
            }
        }
    }

    IEnumerator SpawnArrow()
    {
        isSpawn = true;
        blockInput = true;
        OnActiveTarget(true);
        PlayScreen.Instance.SetActiveWatchArrowText(true);
        float delay = level.nodes[idxOfNode].time / level.nodes[idxOfNode].numOfNode;
        yield return Yielders.Get(1);
        while (idxOfArrow < level.nodes[idxOfNode].numOfNode)
        {
            int type = level.nodes[idxOfNode].arrows[idxOfArrow];
            if (type > 3)
                type = Random.Range(0, 4);
            var obj = SimplePool.Spawn(prefab);
            obj.transform.position = posStart[type];
            obj.Init(type, level.speed);
            listArrow.Add(obj);
            canSpawn = false;

            yield return Yielders.Get(0.2f);
            lsButton[type].AnimationState.SetAnimation(0, "action", true);
            lsButton[type].AnimationState.AddAnimation(0, "idle", false, 0);
            //listArrow.Add(obj);
            //if (lsEffectObjectMoveDown[type].AnimationName != "idle" || lsEffectObjectMoveDown[type].AnimationName != "in")
            //{
            //    lsEffectObjectMoveDown[type].AnimationState.SetAnimation(0, "in", false);
            //    lsEffectObjectMoveDown[type].AnimationState.AddAnimation(0, "idle", true, 0);
            //}
            idxOfArrow++;
            yield return new WaitUntil(() => canSpawn == true);
        }
        //yield return new WaitForSeconds(1.6f - (level.speed-1f));
        isSpawn = false;
        blockInput = false;
        currentState = STATE.ANSWER_STATE;
        answerPool.gameObject.SetActive(true);
        answerPool.CreateNewPool(level.nodes[idxOfNode].numOfNode);
        PlayScreen.Instance.SetActiveWatchArrowText(false);
        PlayScreen.Instance.ShowCountDown(level.nodes[idxOfNode].time);
        PlayScreen.Instance.SetActiveRepeatText(true);
        yield return 0;
    }
    public void OnArrowDisappear(Arrow obj)
    {
        StartCoroutine(ArrowDisappear(obj));
    }
    IEnumerator ArrowDisappear(Arrow obj)
    {
        yield return Yielders.Get(0.2f);
        obj.SetActive(false);
        canSpawn = true;
        listArrow.Remove(obj);
        SimplePool.Despawn(obj.gameObject);
    }
    void OnActiveTarget(bool isActive)
    {
        groupTap.SetActive(isActive);
       
        //objTarget.SetActive(isActive);
        //skeEffectScore.gameObject.SetActive(isActive);
            //objTarget.transform.position = new Vector3(0, -3.5f);
    }
    void OnTouchDown(int type)
    {
        //objTut.SetActive(false);

        lsButton[type].AnimationState.SetAnimation(0, "action", true);
        lsButton[type].AnimationState.AddAnimation(0, "idle", false, 0);
        PlayScreen.Instance.OnTapButtonDown(type);
        // Context.selectedButton = type;

        if (idxOfAnswer < level.nodes[idxOfNode].numOfNode)
        {
            answerPool.UpdateAnswer(type, idxOfAnswer);
            if (type == level.nodes[idxOfNode].arrows[idxOfAnswer])
            {
                idxOfAnswer++;
                if (idxOfAnswer == level.nodes[idxOfNode].numOfNode)
                {
                    StartCoroutine(CorrectAnswer());
                }
            }
            else
            {
                StartCoroutine(IncorectArrow());
            }
        } else
        {
            if (answerCount == 0) textPerfect.Setup(0);
            else if (answerCount > 0 && answerCount < 3) textPerfect.Setup(1);
            PlayScreen.Instance.OnPerfect();
            NextNode();
        }
       
    }
    void OnTouchUp()
    {
        // objTut.SetActive(true);
        for (int i = 0; i < lsButton.Length; i++)
            lsButton[i].AnimationState.SetAnimation(0, "idle", false);
    }

    public void NextNode()
    {
        answerCount = 0;
        answerPool.gameObject.SetActive(false);
        OnTouchUp();
        PlayScreen.Instance.CloseCountDown();
        PlayScreen.Instance.SetActiveRepeatText(false);
        idxOfNode++;
        PlayScreen.Instance.UpdateProgress(idxOfNode * 1f / level.numOfNodes);
        idxOfArrow = 0;
        idxOfAnswer = 0;
        currentState = STATE.ASK_STATE;
    }
    IEnumerator IncorectArrow()
    {
        blockInput = true;
        move--;
        if (move < 0) move = 0;
        OnTouchUp();
        PlayScreen.Instance.CloseCountDown();
        PlayScreen.Instance.IncorectAnswer();
        Instantiate(incorect, transform.position + Vector3.up, Quaternion.identity);
        yield return Yielders.Get(1);
        PlayScreen.Instance.ShowCountDown(level.nodes[idxOfNode].time);
        blockInput = false;
        answerPool.ClearAnswer();
        idxOfAnswer = 0;
        if (answerCount < 3) answerCount++;
        yield return 0;
    }
    IEnumerator CorrectAnswer()
    {
        yield return Yielders.Get(0.4f);
        move++;
        if (move > 1)
        {
            PlayScreen.Instance.DoubleCorectAnswer();
            move = 0;
        }
        if (answerCount == 0) textPerfect.Setup(0);
        else if (answerCount > 0 && answerCount < 3) textPerfect.Setup(1);
        PlayScreen.Instance.OnPerfect();
        NextNode();
        yield return 0;
    }
    IEnumerator OverTime()
    {
        blockInput = true;
        isOverTime = true;
        move--;
        if (move < 0) move = 0;
        OnTouchUp();
        Instantiate(incorect, transform.position + Vector3.up, Quaternion.identity);
        yield return Yielders.Get(1);
        PlayScreen.Instance.ShowCountDown(level.nodes[idxOfNode].time);
        blockInput = false;
        isOverTime = false;
        answerPool.ClearAnswer();
        idxOfAnswer = 0;
        if (answerCount < 3) answerCount++;
        yield return 0;
    }
    public void OnEnd(bool isPause)
    {
        enable = false;

        if (!isPause)
        {
            for (int i = 0; i < listArrow.Count; i++)
            {
                SimplePool.Despawn(listArrow[i].gameObject);
            }
            listArrow.Clear();
            StopAllCoroutines();
            AudioManager.Instance.StopGame();
        }
        else
            AudioManager.Instance.PauseGame();
        answerPool.gameObject.SetActive(false);
        OnActiveTarget(false);
    }
    public void OnWinGame()
    {
        OnEnd(false);
        PlayScreen.Instance.OnWin();
    }
}
