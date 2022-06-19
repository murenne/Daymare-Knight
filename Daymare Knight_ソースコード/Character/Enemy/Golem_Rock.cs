using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem_Rock : MonoBehaviour
{
    public enum  RockStates{HitPlayer,HitEnemy,HitNothing}
    public RockStates rockStates;
    private Rigidbody rb;

    [Header("Basic Setting")]
    public float force;
    public int damage;
    public GameObject target;
    private Vector3 direction;
    public GameObject brokenRocks;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    void FixedUpdate() 
    {
        if(rb.velocity.sqrMagnitude < 1 && rb.velocity.sqrMagnitude > 0)
        {
            rockStates = RockStates.HitNothing;
        }
        
    }

    public void FlyToTarget()
    {
        if(target == null)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }

        direction = (target.transform.position - transform.position + Vector3 .up).normalized;
        rb.AddForce(direction*force,ForceMode.Impulse);
    }


    void OnCollisionEnter(Collision other)
    {
        switch(rockStates)
        {
            case RockStates.HitPlayer:
                if(other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                 other.gameObject.GetComponent<CharacterStates>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStates>());

                    rockStates = RockStates.HitNothing;
                    Destroy(this,5f);
                }
            break;

            case RockStates.HitEnemy:
            
                if(other.gameObject.CompareTag("Enemy"))
                {
                    var otherStates = other.gameObject.GetComponent<CharacterStates>();
                    otherStates.TakeDamage(damage,otherStates);
                    Instantiate(brokenRocks,transform.position,Quaternion.identity);
                    Destroy(gameObject);

                }

            break;

        }
        
    }
}
