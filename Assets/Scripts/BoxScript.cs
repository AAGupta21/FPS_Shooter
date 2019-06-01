using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField] private GameObject[] BoxPrefab = null;

    [SerializeField] private float SpawnRadius = 10f;
    [SerializeField] private float SpawnTimeDiff = 0.4f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > SpawnTimeDiff)
        {
            Spawn();
            timer = 0f;
        }
    }

    private void Spawn()
    {
        float random = Random.Range(0f, BoxPrefab.Length);

        Vector3 position = Random.insideUnitSphere * SpawnRadius;
        position.y = 20f;

        Instantiate(BoxPrefab[(int)random], position, Quaternion.identity, transform);
    }
}