using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float TurnSpeed = 10f;

    private bool LookingAround = false;
    private bool MovingAround = false;
    private Vector3 CurrRot = Vector3.zero;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        CurrRot = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        Move();
        
        if (!LookingAround && !MovingAround)
            Time.timeScale = 0.01f;
        else
            Time.timeScale = 1f;
    }

    private void Move()
    {
        Vector3 DirecFor = Camera.main.transform.forward;
        Vector3 DirecRight = Camera.main.transform.right;

        float Input_Ver = Input.GetAxis("Mouse Y");
        float Input_Hor = Input.GetAxis("Mouse X");


        DirecFor.y = 0f;
        DirecRight.y = 0f;

        if (Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f)
        {
            MovingAround = false;
        }
        else
        {
            MovingAround = true;
            transform.position += DirecFor * MoveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical");
            transform.position += DirecRight * MoveSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal");
        }

        if (Input_Ver == 0f && Input_Hor == 0f)
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

            transform.GetChild(0).rotation = Quaternion.Euler(CurrRot);
        }

    }
}
