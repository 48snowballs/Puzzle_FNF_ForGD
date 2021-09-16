using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Sprite[] spr;
    [SerializeField] private SpriteRenderer img;
    [SerializeField] private float speedMoveDefault;
    [HideInInspector] public int type = 0;

    public Transform trans;
       
    private float currentSpeed;
    private bool active;
    private bool first;
    private bool first1;

    private void OnEnable()
    {
        if (trans == null)
            trans = transform;
    }
    public void Init(int type, float speed = 1)
    {
        this.type = type;
        img.sprite = spr[type];
        img.color = new Color(1, 1, 1, 1);
        currentSpeed = speedMoveDefault * speed;
        active = true;
        first = true;
        first1 = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
            trans.Translate(0, currentSpeed * Time.deltaTime, 0);
        if (trans.position.y > 0 && active)
        {
            if (first)
            {
                first = false;
                PlayScreen.Instance.OnEnemyHit(type);
            } else
            {
                if (trans.position.y > 2f && first1)
                {
                    //  active = false;
                    first1 = false;
                    img.DOPause();
                    img.DOColor(new Color(1, 1, 1, 0), 0.2f);
                    EvenGlobalManager.Instance.OnArrowDisappear.Dispatch(this);
                  //  SimplePool.Despawn(gameObject);
                }
            }          
        }
    }
    public void SetActive(bool active)
    {
        this.active = active;
    }
}
