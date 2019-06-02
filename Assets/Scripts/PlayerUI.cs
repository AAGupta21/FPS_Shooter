using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private float timer = 60f;
    
    [SerializeField] private Text TimerText = null;
    [SerializeField] private Text BulletsText = null;
    [SerializeField] private Text GrenadesText = null;


    private void Update()
    {
        timer -= (Time.deltaTime * Time.timeScale);

        TimerText.text = timer.ToString();
    }

    public void BulletCount(float Bullets, float Grenades)
    {
        BulletsText.text = "/ " + Bullets.ToString();
        GrenadesText.text = Grenades.ToString();
    }
}