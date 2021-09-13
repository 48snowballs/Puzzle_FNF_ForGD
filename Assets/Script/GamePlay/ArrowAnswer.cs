using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnswer : MonoBehaviour
{
    [SerializeField] private Sprite[] spr;
    [SerializeField] private SpriteRenderer img;

    private void Start()
    {
        ChangeType(-1);
    }
    public void ChangeType(int type)
    {
        if (type >= 0)
        {
            img.sprite = spr[type];
        } else
        {
            img.sprite = null;
        }
    }
}
