using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState {   Idle, Collect_Gun, Collect_Grenade, Chase_Player, Shoot};

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float DelayToActions = 0.2f;
    [SerializeField] private float DelayAfterShoot = 0.3f;

    [SerializeField] private GameObject BulletsPrefab = null;
    [SerializeField] private GameObject GrenadePrefab = null;
    [SerializeField] private float ShootRange = 10f;
    [SerializeField] private float ItemSearchRadius = 20f;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Transform ShootPos = null;
    [SerializeField] private float ThrowForce = 50f;
    [SerializeField] private LayerMask layer = 0;

    private Transform PlayerTransform = null;
    private Vector3 TargetPos = Vector3.zero;
    private bool Moving = false;
    private bool ChasingPlayer = false;
    private GameObject TargetObj = null;

    public int BulletCnt = 0;
    public int GrenadeCnt = 0;

    private AIState State = AIState.Idle;     // 0 : Idle  1: Going For Gun. 2: Going for Grenade. 3: Getting In Player Range. 4. Shooting.

    private void OnEnable()
    {
        if(PlayerTransform == null)
        {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if(State == AIState.Idle && !Moving)
        {
            StopAllCoroutines();

            if(BulletCnt > 0)
            {
                if(Vector3.Distance(transform.position, PlayerTransform.position) < ShootRange)
                {
                    State = AIState.Shoot;
                    StartCoroutine(Shoot());
                }
                else
                {
                    if(BulletCnt >= 6 && GrenadeCnt >= 2)
                    {
                        State = AIState.Chase_Player;
                        StartCoroutine(GetInPlayerRange());
                    }
                    else
                    {
                        if(BulletCnt < 6)
                        {
                            State = AIState.Collect_Gun;
                            StartCoroutine(CollectGun());
                        }

                        if(GrenadeCnt < 2)
                        {
                            State = AIState.Collect_Grenade;
                            StartCoroutine(CollectGrenade());
                        }
                    }
                }
            }
            else
            {
                if(Vector3.Distance(transform.position, PlayerTransform.position) < ShootRange)
                {
                    if (GrenadeCnt > 0)
                    {
                        State = AIState.Shoot;
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    State = AIState.Collect_Gun;
                    StartCoroutine(CollectGun());
                }
            }
        }

        if (Moving)
        {
            if (!ChasingPlayer)
            {
                agent.SetDestination(TargetPos);
                
                if ((BulletCnt > 0 || GrenadeCnt > 0) && Vector3.Distance(transform.position, PlayerTransform.position) < ShootRange && agent.remainingDistance != 0f)
                {
                    agent.ResetPath();
                    TargetObj = null;
                    Moving = false;
                    State = AIState.Idle;
                }

                if((TargetObj == null) || (TargetObj!= null && !TargetObj.activeInHierarchy))
                {
                    agent.ResetPath();
                    TargetObj = null;
                    Moving = false;
                    State = AIState.Idle;
                }
            }
            else
            {
                agent.SetDestination(PlayerTransform.position);
                
                if (agent.remainingDistance < ShootRange && agent.remainingDistance != 0f)
                {
                    agent.ResetPath();
                    ChasingPlayer = false;
                    Moving = false;
                    State = AIState.Idle;
                }

                if (agent.remainingDistance > ShootRange && (BulletCnt < 6 || GrenadeCnt < 2))
                {
                    agent.ResetPath();
                    ChasingPlayer = false;
                    Moving = false;
                    State = AIState.Idle;
                }
            }
        }
    }

    IEnumerator CollectGun()
    {
        Collider[] colItem = Physics.OverlapSphere(transform.position, ItemSearchRadius);

        Collider temp = null;

        foreach(Collider col in colItem)
        {
            if (col.gameObject.tag == "Gun")
            {
                if (temp == null)
                    temp = col;

                if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, temp.transform.position))
                    temp = col;
            }
        }

        if (temp != null)
        {
            TargetPos = temp.transform.position;
            TargetPos.y = 1.5f;
            TargetObj = temp.gameObject;
            yield return new WaitForSeconds(DelayToActions);
            Moving = true;
        }

        State = AIState.Idle;
    }

    IEnumerator CollectGrenade()
    {
        Collider[] colItem = Physics.OverlapSphere(transform.position, ItemSearchRadius);

        Collider temp = null;

        foreach (Collider col in colItem)
        {
            if (col.gameObject.tag == "Grenade")
            {
                if (temp == null)
                    temp = col;

                if (Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, temp.transform.position))
                    temp = col;
            }
        }

        if (temp != null)
        {
            TargetPos = temp.transform.position;
            TargetPos.y = 1.5f;
            TargetObj = temp.gameObject;
            yield return new WaitForSeconds(DelayToActions);
            Moving = true;
        }

        State = AIState.Idle;
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(DelayToActions);

        Vector3 temp = PlayerTransform.position;
        temp.y = transform.position.y;

        transform.LookAt(temp);

        if (BulletCnt > 0 && GrenadeCnt > 0)
        {
            float random = Random.Range(0f, 3f);
            if (random < 2f)
            {
                Instantiate(BulletsPrefab, ShootPos.position, transform.rotation);
                BulletCnt--;
            }
            else
            {
                GameObject g = Instantiate(GrenadePrefab, ShootPos.position, GrenadePrefab.transform.rotation);
                g.GetComponent<Rigidbody>().AddForce(ThrowForce * (PlayerTransform.position - transform.position).normalized, ForceMode.Impulse);
                GrenadeCnt--;
            }
        }
        else
        {
            if (BulletCnt > 0)
            {
                Instantiate(BulletsPrefab, ShootPos.position, transform.rotation);
                BulletCnt--;
            }

            if (GrenadeCnt > 0)
            {
                GameObject g = Instantiate(GrenadePrefab, ShootPos.position, GrenadePrefab.transform.rotation);
                g.GetComponent<Rigidbody>().AddForce(ThrowForce * (PlayerTransform.position - transform.position).normalized, ForceMode.Impulse);
                GrenadeCnt--;
            }
        }

        yield return new WaitForSeconds(DelayAfterShoot);
        State = AIState.Idle;
    }

    IEnumerator GetInPlayerRange()
    {
        yield return new WaitForSeconds(DelayToActions);
        Moving = true;
        ChasingPlayer = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Gun")
        {
            BulletCnt += 6;
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Grenade")
        {
            GrenadeCnt++;
            collision.gameObject.SetActive(false);
        }
    }
}