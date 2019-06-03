using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 25f;
    [SerializeField] private float BulletMass = 1f;

    private void Update()
    {
        transform.position += transform.forward * BulletSpeed * Time.deltaTime * Time.timeScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
            rb.AddForceAtPosition(transform.forward * BulletMass * BulletSpeed, transform.position + (other.gameObject.transform.position - transform.position).normalized * 0.025f);

        Destroy(this.gameObject);
    }
}
