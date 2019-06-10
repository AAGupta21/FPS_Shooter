using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy = null;

    private Transform PlayerTransform = null;

    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        
    }

    void SpawnChecker()
    {

    }

    void Spawn()
    {
        Vector3 temp = CalculatePoint();
        while (Vector3.Distance(temp, PlayerTransform.position) < 5f)
            temp = CalculatePoint();

        Instantiate(enemy, temp, Quaternion.identity);
    }

    Vector3 CalculatePoint()
    {
        Vector3 Pos = new Vector3( Random.Range(-25f, 25f), 1.5f, Random.Range(-25f, 25f));
        return Pos;
    }
}
