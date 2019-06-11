using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public static float timer = 60f;
    
    [SerializeField] private Text TimerText = null;
    [SerializeField] private Text BulletsText = null;
    [SerializeField] private Text GrenadesText = null;
    [SerializeField] private Material Screen = null;
    [SerializeField] private GameObject SpawnerE = null;
    [SerializeField] private GameObject SpawnerO = null;
    [SerializeField] private GameObject VictoryPage = null;
    [SerializeField] private GameObject DefeatPage = null;

    public static bool StopGame = false;

    private void OnEnable()
    {
        Screen.color = Color.clear;
        VictoryPage.SetActive(false);
        DefeatPage.SetActive(false);
    }

    private void Update()
    {
        if(!StopGame)
        {
            timer -= (Time.deltaTime * Time.timeScale);

            TimerText.text = timer.ToString();

            if (timer <= 0f)
            {
                Color col = Screen.color;
                col.b = 200;
                col.a = 0.8f;
                Screen.color = col;

                VictoryPage.SetActive(true);
                DisableGame();
            }

            if(!PlayerHealth.Alive)
            {
                Color col = Screen.color;
                col.r = 200;
                col.a = 0.8f;
                Screen.color = col;

                DefeatPage.SetActive(true);
                DisableGame();
            }
        }
    }

    private void DisableGame()
    {
        Cursor.visible = true;
        StopGame = true;
        GameObject[] g = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject x in g)
            Destroy(x);
        
        SpawnerE.SetActive(false);
        SpawnerO.SetActive(false);
        TimerText.text = "";
        BulletsText.text = "";
        GrenadesText.text = "";

    }

    public void BulletCount(float Bullets, float Grenades)
    {
        BulletsText.text = Bullets.ToString();
        GrenadesText.text = "/ " + Grenades.ToString();
    }

    public void RetryGame()
    {
        Scene scene = SceneManager.GetActiveScene();

        SpawnerE.SetActive(true);
        SpawnerO.SetActive(true);
        PlayerHealth.Alive = true;
        StopGame = false;
        timer = 60f;

        SceneManager.LoadScene(scene.buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}