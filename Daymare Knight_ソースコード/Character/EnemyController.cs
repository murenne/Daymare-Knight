using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates{GUARD,PATROL,CHASE,DEAD}
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator animator;

    protected CharacterStates characterStates;
    private Collider coll;


    [Header("Basic Settings")]
    public float sightRadius;
    protected GameObject attackTarget;
    public bool isGuard;
    private float speed;
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;
    private Quaternion guardRotation;


    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPosePoint;
    



    //配合动画转换
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool isPlayerDead;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        animator = GetComponent<Animator>();
        guardPosePoint = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
        coll = GetComponent<Collider>();
        characterStates = GetComponent<CharacterStates>();
    }

    void Start()
    {
        if(isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();//得到初始移动的点
            GameManager.Instance.AddObserver(this);//FIXME
        }
    }

    //添加和移除列表
    //void OnEnable() 
    //{
   //     GameManager.Instance.AddObserver(this); 
   // }
    void OnDisable() 
    {    
        if(!GameManager.IsInitialized)
        {
            return;
        }
        
         GameManager.Instance.RemoveObserver(this);

         if(GetComponent<LoopSpawner>() && isDead )
         {
            GetComponent<LoopSpawner>().SpawnLoot();
         }
       
    }

      void Update()
    {
        if(characterStates.CurrentHealth == 0)
        {
            isDead = true;
        }
        if(!isPlayerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation()
    {
        animator.SetBool("Walk",isWalk);
        animator.SetBool("Chase",isChase);
        animator.SetBool("Follow",isFollow);
        animator.SetBool("Critical",characterStates.isCritical);
        animator.SetBool("Death",isDead);
    }

    void SwitchStates()
    {
        if(isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }

        //发现player就切换到chase
        else if(FindPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;

                if(transform.position != guardPosePoint)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPosePoint;

                    if(Vector3.Distance(transform.position,guardPosePoint) <= agent.stoppingDistance)// squrMagnitude()也可以用
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.05f);//lerp缓慢转头
                    }
                }

                break;

            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;

                if(Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance )
                {
                    isWalk = false;
                    if(remainLookAtTime > 0)
                    {
                        remainLookAtTime -=Time.deltaTime;
                    }
                    else
                    {
                        GetNewWayPoint();
                    }
                    
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;

            case EnemyStates.CHASE:
            //追player，拉脱，动画，攻击
                isWalk = false;
                isChase = true;

                agent.speed = speed;

                if(!FindPlayer())
                {
                    //回到上一个状态
                    isFollow = false;
                    if(remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;//立刻停止
                        remainLookAtTime -=Time.deltaTime;
                    }
                    else if(isGuard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                    { 
                        enemyStates = EnemyStates.PATROL;
                    }     
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position ;
                }
                //判断是否在攻击范围之内
                if(TargetInAttackRange() || TargetInSkillkRange())
                {
                    isFollow = false;
                    agent.isStopped = true;

                    if(lastAttackTime < 0)
                    {
                        lastAttackTime = characterStates.attackData.coolDownTime;
                        //暴击判断
                        characterStates.isCritical = Random.value < characterStates.attackData.criticalChance;
                        //执行攻击
                        Attack();

                    }
                }



                break;

            case EnemyStates.DEAD:
                coll.enabled = false;
                //agent.enabled = false;
                agent.radius = 0;
                Destroy(gameObject ,2f);
                break;
            
        }

    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if(TargetInAttackRange())
        {
            //近身攻击动画
            animator.SetTrigger("Attack");

        }
         if(TargetInSkillkRange())
        {
            //技能攻击动画
            animator.SetTrigger("Skill");
        }
    }

    bool FindPlayer()
    {
        //周围球体范围内有没有player
        var colliders = Physics.OverlapSphere(transform.position ,sightRadius);

        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }

        }
        attackTarget = null;
        return false;

    }

    bool TargetInAttackRange()
    {
        if(attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position,transform.position) <= characterStates.attackData.attackRange;
        }
        else
        {
            return false;
        }

    }

    bool TargetInSkillkRange()
    {
         if(attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position,transform.position) <= characterStates.attackData.SkillRange;
        }
        else
        {
            return false;
        }
    }

    void   GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;//还原时间
        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange,patrolRange);
        Vector3 randowPoint = new Vector3 (guardPosePoint.x + randomX, transform.position.y,guardPosePoint.z + randomZ);//y方向不变因为地形不是平坦的
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randowPoint,out hit , patrolRange,1) ? hit.position : transform.position ;//返回bool类型

    }

    void  OnDrawGizmosSelected() //可视化视野范围
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }

    //Animation event
    void Hit()
    {
        if(attackTarget != null && transform.IsFacingTarget(attackTarget.transform)) 
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates,targetStates);
        }  
       
    }

    public void EndNotify()
    {
        //获胜动画，停止移动，停止agent
        animator.SetBool("Win",true);
        isPlayerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
  

