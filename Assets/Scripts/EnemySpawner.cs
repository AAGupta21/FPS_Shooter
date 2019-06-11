using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy = null;
    [SerializeField] private Vector2Int[] Array = null;

    private Transform PlayerTransform = null;
    private int Index = 0;

    private void OnEnable()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Index < Array.Length)
        {
            if (Array[Index].x >= PlayerUI.timer)
            {
                for (int i = 0; i < Array[Index].y; i++)
                    Spawn();
                
                Index++;
            }
        }
    }

    void Spawn()
    {
        Vector3 temp = CalculatePoint();
        while (Vector3.Distance(temp, PlayerTransform.position) < 5f)
            temp = CalculatePoint();

        GameObject g = Instantiate(enemy, temp, Quaternion.identity);

        if (PlayerUI.timer < 20f)
            g.GetComponent<EnemyAI>().BulletCnt = 6;
    }

    Vector3 CalculatePoint()
    {
        Vector3 Pos = new Vector3( Random.Range(-25f, 25f), 1.5f, Random.Range(-25f, 25f));
        return Pos;
    }
}