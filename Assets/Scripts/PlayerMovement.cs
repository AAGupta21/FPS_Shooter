using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float TurnSpeed = 10f;
    [SerializeField] private PlayerUI playerUI = null;

    private Vector3 CurrRot = Vector3.zero;

    private bool LookingAround = false;
    private bool MovingAround = false;

    private int Bullets = 02;
    private int Grenades = 03;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        CurrRot = transform.rotation.eulerAngles;
        playerUI.BulletCount(Bullets, Grenades);
    }

    private void Update()
    {
        LateralMovement();
        MouseMovement();

        if (Input.GetMouseButtonUp(0))
            ShootBullets();

        if (Input.GetMouseButtonUp(1))
            ThrowGrenade();

        if (!LookingAround && !MovingAround)
            Time.timeScale = 0.01f;
        else
            Time.timeScale = 1f;
    }

    private void LateralMovement()
    {
        Vector3 DirecFor = transform.forward;
        Vector3 DirecRight = transform.right;

        DirecFor.y = 0f;
        DirecRight.y = 0f;

        if(Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f)
        {
            MovingAround = false;
        }
        else
        {
            MovingAround = true;
            transform.localPosition += DirecFor * MoveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical");
            transform.localPosition += DirecRight * MoveSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal");
        }
    }

    private void MouseMovement()
    {
        float Input_Ver = Input.GetAxis("Mouse Y");
        float Input_Hor = Input.GetAxis("Mouse X");
        
        if(Input_Ver == 0f && Input_Hor == 0f)
        {
            LookingAround = false;
        }
        else
        {
            LookingAround = true;
            CurrRot += new Vector3(Input_Ver * TurnSpeed * -1, Input_Hor * TurnSpeed, 0f);

            if (CurrRot.x > 40f)
                CurrRot.x = 40f;

            if (CurrRot.x < -60f)
                CurrRot.x = -60f;

            transform.rotation = Quaternion.Euler(CurrRot);
        }
    }

    private void ThrowGrenade()
    {
        if (Grenades > 0)
        {
            Debug.Log("Throw Grenade!");
            Grenades--;
            playerUI.BulletCount(Bullets, Grenades);
        }
    }

    private void ShootBullets()
    {
        if(Bullets > 0)
        {
            Debug.Log("Bullet");
            Bullets--;
            playerUI.BulletCount(Bullets, Grenades);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Grenade")
        {
            Grenades++;
            collision.gameObject.SetActive(false);
        }

        if(collision.gameObject.tag == "Gun")
        {
            Bullets += 6;
            collision.gameObject.SetActive(false);
        }


    }
}