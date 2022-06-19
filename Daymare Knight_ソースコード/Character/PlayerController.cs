using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private CharacterStates characterStates;
    private GameObject attackTarget;
    private float lastAttackTime; // 上一次攻击时间计时器
    private  bool isDead;

    public float stopDistance;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
        stopDistance = agent.stoppingDistance;
    }
    void OnEnable() 
    {
        MouseManagement.Instance.onMouseClicked += MoveToTarget;
        MouseManagement.Instance.onEnemyClicked += EventAttack;
        GameManager.Instance.RigisterPlayer(characterStates);
    }

    void Start() 
    {
        //MouseManagement.Instance.onMouseClicked += MoveToTarget;
        //MouseManagement.Instance.onEnemyClicked += EventAttack;
        //characterStates.MaxHealth = 2;
        //GameManager.Instance.RigisterPlayer(characterStates);
        SaveManagement.Instance.LoadPlayerData();
    }

    void OnDisable() 
    {
        if(!MouseManagement.IsInitialized)
        {
            return;
        }
        
        MouseManagement.Instance.onMouseClicked -= MoveToTarget;
        MouseManagement.Instance.onEnemyClicked -= EventAttack;
    }

  

    void Update() 
    {
        isDead = characterStates.CurrentHealth == 0;

        if(isDead)
        {
            GameManager.Instance.NOtifyObservers();
        }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
        
    }

    private void SwitchAnimation() //切换动画
    {
        animator.SetFloat("Speed",agent.velocity.sqrMagnitude);
        animator.SetBool("Death",isDead);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();  //打断攻击

        if(isDead)
        {
            return;
        }
        
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target; //移动
    }

      private void EventAttack(GameObject target)
    {
        if(isDead)
        {
            return;
        }
        
        if(target != null) //移动到目标面前攻击
        {
            attackTarget = target;
            characterStates.isCritical = UnityEngine.Random.value < characterStates.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStates.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        
        while(Vector3.Distance(attackTarget.transform.position,transform.position) > characterStates.attackData.attackRange) //大于攻击距离
        {
            agent.destination = attackTarget.transform .position;
            yield return null;
        }

        agent.isStopped = true;

        //attack
        if(lastAttackTime < 0)
        {
            animator.SetBool("Critical",characterStates.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = characterStates.attackData.coolDownTime;
        }
    }

    //Animation event
    void Hit()
    {
        if(attackTarget.CompareTag("Attackable"))
        {
            if(attackTarget.GetComponent<Golem_Rock>())
            {
                attackTarget.GetComponent<Golem_Rock>().rockStates = Golem_Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse); //可以调整成变量
            }

        }
        else
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates,targetStates);
        }
        
    }
}
