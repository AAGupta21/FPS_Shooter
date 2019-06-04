using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCan : MonoBehaviour
{
    [SerializeField] private float ExplosionForce = 350f;
    [SerializeField] private float ExplosionRadius = 8f;
    [SerializeField] private float UpwardForce = 8f;
    [SerializeField] private GameObject ExplosionEffect = null;
    [SerializeField] private LayerMask layer = 0;
    [SerializeField] private float ExplodingCanDamage = 20f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
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
                if (rb.gameObject.tag == "Player")
                {
                    Ray ray = new Ray(transform.position, (rb.position - transform.position).normalized);

                    if (!Physics.Raycast(ray, Vector3.Distance(transform.position, rb.position), layer))
                    {
                        PlayerHealth.Health -= ExplodingCanDamage;
                    }
                }
                else
                {
                    rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                    rb.AddRelativeForce(Vector3.up * UpwardForce, ForceMode.Impulse);
                }
            }
        }

        GameObject g = Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation);

        g.GetComponent<ParticleSystem>().Play();
        
        gameObject.SetActive(false);
    }
}