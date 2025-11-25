using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class MonsterAI : MonoBehaviour
{
    float health = 10f;
    [Header("Movement Settings")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float wanderSpeed = 2f;
    
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 4f;
    [SerializeField] private float attackCooldown = 2f;
    
    private NavMeshAgent agent;
    private Transform player;
    private float wanderTimerCurrent;
    private float attackTimerCurrent;
    private bool isChasing = false;
    bool isplayer = false;
    bool isAttacking = false;
    Animator animator;
    PlayerControl playerControl;
    Renderer monsterRenderer;
    
    private enum State
    {
        Wandering,
        Chasing,
        Attacking
    }
    
    private State currentState = State.Wandering;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerControl = FindAnyObjectByType<PlayerControl>();
        animator = GetComponent<Animator>();
        monsterRenderer = GetComponent<Renderer>();
        wanderTimerCurrent = wanderTimer;
        attackTimerCurrent = 0f;
        
        // 초기 속도 설정
        agent.speed = wanderSpeed;
    }
    
    void Update()
    {
        // 플레이어 감지
        DetectPlayer();
        
        // 공격 쿨다운 감소
        if (attackTimerCurrent > 0)
        {
            attackTimerCurrent -= Time.deltaTime;
        }
        
        // 상태에 따른 행동
        switch (currentState)
        {
            case State.Wandering:
                Wander();
                break;
            case State.Chasing:
                ChasePlayer();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
        }
    }
    
    void DetectPlayer()
    {
        if(isAttacking) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        
        if (hits.Length > 0)
        {
            player = hits[0].transform;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= attackRange)
            {
                currentState = State.Attacking;
            }
            else
            {
                currentState = State.Chasing;
                agent.speed = chaseSpeed;
            }
        }
        else
        {
            player = null;
            currentState = State.Wandering;
            agent.speed = wanderSpeed;
        }
    }
    
    void Wander()
    {
        wanderTimerCurrent += Time.deltaTime;
        animator.SetBool("IsMoving", false);
        if (wanderTimerCurrent >= wanderTimer)
        {
            animator.SetBool("IsMoving", true);
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            wanderTimerCurrent = 0;
        }
    }
    
    void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            animator.SetBool("IsMoving", true);

        }
    }
    
    void AttackPlayer()
    {
        // 플레이어를 바라보기
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            animator.SetBool("IsMoving", false);


            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            // 공격 실행
            if (attackTimerCurrent <= 0 && !isAttacking)
            {
                isAttacking = true;
                attackTimerCurrent = attackCooldown;
                PerformAttack();
            }
        }
    }


    void PerformAttack()
    {
        Debug.Log("몬스터가 공격!");
        
        // 여기에 공격 애니메이션 트리거 추가
        animator.SetTrigger("Attack");
        
        // 플레이어에게 데미지 적용
        if (player != null)
        {
            Invoke("PlayerCheck", 1f);
            // //PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            // if (playerHealth != null)
            // {
            //     playerHealth.TakeDamage(attackDamage);
            // }

            if (isplayer)
            {
                // StartCoroutine(playerControl.Damaged(attackDamage));
                playerControl.Damaged(attackDamage);
            }
            Invoke("EndAttack", attackCooldown);
        }
    }
    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        if(player != null)
        {
            // 플레이어가 공격 범위를 벗어났는지 확인
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > attackRange)
            {
                currentState = State.Chasing;
            }
        }
        else
        {
            currentState = State.Wandering;
        }
    }

    public void Damaged(float value)
    {
        health -= value;
        monsterRenderer.material.color = Color.red;

    }
    
    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);
        
        return navHit.position;
    }
    
    // Gizmo로 감지 범위 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward*attackRange);
    }
    void PlayerCheck()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Debug.Log("you hitted");
                isplayer = true;
            }
            else {isplayer =false;}
        }
    }
}