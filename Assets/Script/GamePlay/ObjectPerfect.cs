using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPerfect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Sprite[] lsSprite;

    public void Setup(int state)
    {
        image.sprite = lsSprite[state];
        gameObject.SetActive(true);

        DOTween.Pause(gameObject);
        DOTween.Pause(image);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        transform.DOScale(1, 0.2f).SetEase(Ease.Flash);
        image.color = new Color(1, 1, 1, 0);
        image.DOFade(1, 0.2f);
        image.DOFade(0, 0.7f).SetDelay(0.7f).OnComplete(() => SimplePool.Despawn(gameObject));
    }
}
