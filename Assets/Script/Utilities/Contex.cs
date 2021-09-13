using System.Collections;
using System;
using UnityEngine;

public class Contex : MonoBehaviour
{
    public static IEnumerator IECallback(float delay, Action callback)
    {
        yield return Yielders.Get(delay);
        callback();
    }
}
