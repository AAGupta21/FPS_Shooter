using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCan : MonoBehaviour
{
    [SerializeField] private float ExplosionForce = 350f;
    [SerializeField] private float ExplosionRadius = 8f;
    [SerializeField] private float UpwardForce = 8f;
    [SerializeField] private GameObject ExplosionEffect = null;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
            Explode();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.name == "Explosion_Effect")
            Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                rb.AddRelativeForce(Vector3.up * UpwardForce, ForceMode.Impulse);
            }
        }

        GameObject g = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        

        gameObject.SetActive(false);
    }
}
