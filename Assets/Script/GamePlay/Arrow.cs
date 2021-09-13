using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Sprite[] spr;
    [SerializeField] private SpriteRenderer img;
    [SerializeField] private float speedMoveDefault;
    [HideInInspector] public int type = 0;

    public Transform trans;
       
    private float currentSpeed;
    private bool active;
    private bool frist;

    private void OnEnable()
    {
        if (trans == null)
            trans = transform;
    }
    public void Init(int type, float speed = 1)
    {
        this.type = type;
        img.sprite = spr[type];
        currentSpeed = speedMoveDefault * speed;
        active = true;
        frist = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
            trans.Translate(0, currentSpeed * Time.deltaTime, 0);
        if (trans.position.y < 0 && active)
        {
            if (frist)
            {
                frist = false;
                PlayScreen.Instance.OnEnemyHit(type);
            } else
            {
                if (trans.position.y < -3.5f)
                {
                    active = false;
                    //isBonusScore = false;
                    SimplePool.Despawn(gameObject);
                    //controller.RemoveObjectMove(this);
                }
            }          
        }
    }
}
