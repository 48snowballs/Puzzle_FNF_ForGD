using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerPool : MonoBehaviour
{
    public ArrowAnswer prefabs;

    private int num;
    private List<ArrowAnswer> arrowAnswers = new List<ArrowAnswer>();   
    public void CreateNewPool(int n)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        arrowAnswers.Clear();
        num = n;
        if (n <= 5)
        {
            SpawnRow(n, transform.position);
        }
        else
        {
            SpawnRow(5, transform.position + Vector3.up);
            SpawnRow(n-5, transform.position);
        }
    }
    public void SpawnRow(int n, Vector3 pos)
    {
        Vector2 left = pos + n / 2 * Vector3.left;
        if (n % 2 == 0) left += Vector2.right * 0.5f;
        for (int i = 0; i < n; i++)
        {
            ArrowAnswer a = Instantiate(prefabs, left + i * 1f * Vector2.right, Quaternion.identity);
            a.transform.SetParent(transform);
            arrowAnswers.Add(a);
        }
    }
    public void UpdateAnswer(int type,int idx)
    {
        if (idx >= arrowAnswers.Count) return;
        arrowAnswers[idx].ChangeType(type);
    }
    public void ClearAnswer()
    {
        for (int i = 0; i < arrowAnswers.Count; i++)
        {
            arrowAnswers[i].ChangeType(-1);
        }
    }
}
