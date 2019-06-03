using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerUI playerUI = null;
    [SerializeField] private GameObject GrenadePrefab = null;
    [SerializeField] private GameObject BulletPrefab = null;
    [SerializeField] private float ThrowForce = 50f;
    [SerializeField] private GameObject Effect = null;
    
    private int BulletsCount = 0;
    private int GrenadeCount = 0;

    private void Start()
    {
        playerUI.BulletCount(BulletsCount, GrenadeCount);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            ShootBullets();

        if (Input.GetMouseButtonUp(1))
            ThrowGrenade();
    }

    private void ThrowGrenade()
    {
        Instantiate(Effect, transform.position + transform.forward * 10f, Quaternion.identity);

        if (GrenadeCount > 0)
        {
            GameObject g = Instantiate(GrenadePrefab, transform.parent.GetChild(1).GetChild(1).position + transform.parent.GetChild(1).GetChild(1).forward * 0.5f, Quaternion.identity, transform);
            g.GetComponent<Rigidbody>().AddForce(transform.parent.GetChild(1).GetChild(1).forward * ThrowForce, ForceMode.Impulse);
            
            GrenadeCount--;
            playerUI.BulletCount(BulletsCount, GrenadeCount);
        }
    }

    private void ShootBullets()
    {
        if(BulletsCount > 0)
        {
            GameObject g = Instantiate(BulletPrefab, transform.parent.GetChild(1).GetChild(1).position + transform.parent.GetChild(1).GetChild(1).forward * 0.5f, transform.parent.GetChild(1).GetChild(1).rotation, transform);
            BulletsCount--;
            playerUI.BulletCount(BulletsCount, GrenadeCount);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Gun")
        {
            collision.gameObject.SetActive(false);
            BulletsCount += 6;
            playerUI.BulletCount(BulletsCount, GrenadeCount);
        }
        if (collision.gameObject.tag == "Grenade")
        {
            collision.gameObject.SetActive(false);
            GrenadeCount++;
            playerUI.BulletCount(BulletsCount, GrenadeCount);
        }
    }
}