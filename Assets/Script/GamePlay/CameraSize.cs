using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    private void OnEnable()
    {
        float defaultAspect = 720f / 1280f;
        
        float aspect = (Screen.width * 1.0f) / Screen.height;
        if (defaultAspect > aspect)
        {
            Camera.main.orthographicSize = 5 * defaultAspect / aspect;
        }
    }
}
