using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    public static SplashManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GUIManager.Instance.Init();
        
    }
    public void Load()
    {
        StartCoroutine(ToMainScreen());
    }
    IEnumerator ToMainScreen()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)SCENE_INDEX.Gameplay);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
