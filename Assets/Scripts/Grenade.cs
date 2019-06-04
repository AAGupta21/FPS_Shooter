using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float DelayToExplosion = 0.3f;
    [SerializeField] private float ExplosionForce = 200f;
    [SerializeField] private float ExplosionRadius = 5f;
    [SerializeField] private float UpwardForce = 5f;
    [SerializeField] private GameObject ExplosionEffect = null;
    [SerializeField] private LayerMask layer = 0;
    [SerializeField] private float GrenadeDamage = 20f;

    private void OnEnable()
    {
        StartCoroutine(ToExplode());
    }
    
    IEnumerator ToExplode()
    {
        yield return new WaitForSeconds(DelayToExplosion);
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach(Collider col in colliders)
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();

            if(rb != null)
            {
                if (rb.gameObject.tag == "Player")
                {
                    Ray ray = new Ray(transform.position, (rb.position - transform.position).normalized);

                    if (!Physics.Raycast(ray, Vector3.Distance(transform.position, rb.position), layer))
                    {
                        PlayerHealth.Health -= GrenadeDamage;
                    }
                }
                else
                {
                    rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                    rb.AddRelativeForce(Vector3.up * UpwardForce, ForceMode.Impulse);
                }
            }

        }

        GameObject g = Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation, transform);

        g.GetComponent<ParticleSystem>().Play();

        
        Destroy(this.gameObject, g.GetComponent<ParticleSystem>().main.duration);
    }
}