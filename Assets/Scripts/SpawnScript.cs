using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypesOfSpawns
{
    Guns, Grenades, Exploding_Cans, Boxes
}

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private TypesOfSpawns[] Objects = null;
    [SerializeField] private GameObject Grenades = null;
    [SerializeField] private GameObject Guns = null;
    [SerializeField] private GameObject ExplodingCans = null;
    [SerializeField] private GameObject Boxes = null;
    
    [SerializeField] private float SpawnRadius = 10f;
    [SerializeField] private float SpawnTimeDiff = 0.4f;
    [SerializeField] private int MaxGunsCount = 10;
    [SerializeField] private int MaxGrenadeCount = 5;
    [SerializeField] private int MaxExplodingCansCount = 4;
    [SerializeField] private int MaxBoxesCount = 30;
    
    private GameObject[] GrenadesList = null;
    private GameObject[] GunsList = null;
    private GameObject[] ExplodingCansList = null;
    private List<GameObject> BoxesList = null;

    private float TempSpawnDiff = 0f;

    private float timer = 0f;

    private void OnEnable()
    {
        GrenadesList = new GameObject[MaxGrenadeCount];
        ExplodingCansList = new GameObject[MaxExplodingCansCount];
        GunsList = new GameObject[MaxGunsCount];

        BoxesList = new List<GameObject>();
        
        for(int i = 0; i < MaxGrenadeCount; i++)
        {
            GrenadesList[i] = Instantiate(Grenades, Vector3.zero, Quaternion.identity, transform);
            GrenadesList[i].SetActive(false);
        }

        for (int i = 0; i < MaxGunsCount; i++)
        {
            GunsList[i] = Instantiate(Guns, Vector3.zero, Quaternion.identity, transform);
            GunsList[i].SetActive(false);
        }

        for (int i = 0; i < MaxExplodingCansCount; i++)
        {
            ExplodingCansList[i] = Instantiate(ExplodingCans, Vector3.zero, Quaternion.identity, transform);
            ExplodingCansList[i].SetActive(false);
        }

        for (int i = 0; i < MaxBoxesCount; i++)
        {
            BoxesList.Add(Instantiate(Boxes, Vector3.zero, Quaternion.identity, transform));
            BoxesList[i].SetActive(false);
        }

        TempSpawnDiff = SpawnTimeDiff;
    }

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
        float random = Random.Range(0f, Objects.Length);
        GameObject g = null;

        Vector3 position = Random.insideUnitSphere * SpawnRadius;
        position.y = 20f;

        TypesOfSpawns type = Objects[(int)random];

        switch (type)
        {
            case TypesOfSpawns.Boxes:
                int i;

                for (i = 0; i < MaxBoxesCount; i++)
                {
                    if (!BoxesList[i].activeInHierarchy)
                    {
                        g = BoxesList[i];
                        break;
                    }
                }

                if (i == MaxBoxesCount)
                    g = null;

                break;

            case TypesOfSpawns.Guns:
                
                for (i = 0; i < MaxGunsCount; i++)
                {
                    if (!GunsList[i].activeInHierarchy)
                    {
                        g = GunsList[i];
                        break;
                    }
                }

                if (i == MaxGunsCount)
                    g = null;

                break;

            case TypesOfSpawns.Exploding_Cans:
                
                for (i = 0; i < MaxExplodingCansCount; i++)
                {
                    if (!ExplodingCansList[i].activeInHierarchy)
                    {
                        g = ExplodingCansList[i];
                        break;
                    }
                }

                if (i == MaxExplodingCansCount)
                    g = null;

                break;

            case TypesOfSpawns.Grenades:

                for (i = 0; i < MaxGrenadeCount; i++)
                {
                    if (!GrenadesList[i].activeInHierarchy)
                    {
                        g = GrenadesList[i];
                        break;
                    }
                }

                if (i == MaxGrenadeCount)
                    g = null;

                break;

            default:
                Debug.Log("Issue in Case");
                break;
        }

        if (g == null)
        {
            g = Instantiate(Boxes, transform.position, Quaternion.identity, transform);
            BoxesList.Add(g);
            MaxBoxesCount++;
            SpawnTimeDiff = SpawnTimeDiff + 0.1f;
        }
        else
        {
            g.SetActive(true);
            if (SpawnTimeDiff != TempSpawnDiff)
                SpawnTimeDiff = TempSpawnDiff;
        }

        g.transform.position = position;
    }
}