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

    private float CurrTime = 0f;
    private bool Exploded = false;
    
    private void Update()
    {
        if(CurrTime < DelayToExplosion)
        {
            CurrTime += Time.deltaTime;
        }
        else
        {
            if(!Exploded)
                Explode();
        }
    }

    private void Explode()
    {
        Exploded = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach(Collider col in colliders)
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            
            if(rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                rb.AddRelativeForce(Vector3.up * UpwardForce, ForceMode.Impulse);
            }
        }

        GameObject g = Instantiate(ExplosionEffect, transform.position, transform.rotation, transform);

        Destroy(this.gameObject, 1f);
    }
}