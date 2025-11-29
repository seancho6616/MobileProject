using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class MonsterAI : MonoBehaviour
{
    public MonsterState monsterState;
    float health;
    [Header("Movement Settings")]
    [SerializeField] private float wanderTimer = 5f;
    
    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;
    
    private NavMeshAgent agent;
    private Transform player;
    private float wanderTimerCurrent;
    private float attackTimerCurrent;
    public bool isWeapon = false;
    bool isplayer = false;
    bool isAttacking = false;
    bool isDie= false;
    Animator animator;
    PlayerControl playerControl;
    SkinnedMeshRenderer rendererColor;
    [SerializeField]ParticleSystem dieParticle;
    [SerializeField] GameObject body;
    Color basicColor; 
    Color changeColor;
        
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
        rendererColor = GetComponentInChildren<SkinnedMeshRenderer>();

        health = monsterState.heart;
        wanderTimerCurrent = wanderTimer;
        attackTimerCurrent = 0f;
        basicColor = rendererColor.sharedMaterial.color;
        changeColor = Color.softRed;
        // 초기 속도 설정
        agent.speed = monsterState.moveSpeed;
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
        if(isAttacking || isDie) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, monsterState.wanderRadius, playerLayer);
        
        if (hits.Length > 0)
        {
            player = hits[0].transform;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= monsterState.attackRange)
            {
                currentState = State.Attacking;
            }
            else
            {
                currentState = State.Chasing;
                agent.speed = monsterState.moveSpeed;
            }
        }
        else
        {
            player = null;
            currentState = State.Wandering;
            agent.speed = monsterState.moveSpeed;
        }
    }
    
    void Wander()
    {
        wanderTimerCurrent += Time.deltaTime;
        animator.SetBool("IsMoving", true);
        if (wanderTimerCurrent >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, monsterState.wanderRadius);
            agent.SetDestination(newPos);
            wanderTimerCurrent = 0;
            animator.SetBool("IsMoving", false);
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
                // 여기에 공격 애니메이션 트리거 추가
                animator.SetTrigger("Attack");
                isAttacking = true;
                attackTimerCurrent = attackCooldown;
                PerformAttack();
            }
        }
    }


    void PerformAttack()
    {
        Debug.Log("몬스터가 공격!");
        
        
        // 플레이어에게 데미지 적용
        if (player != null && !isDie)
        {
            Invoke("PlayerCheck", 1f);
            if (isplayer)
            {
                playerControl.Damaged(monsterState.attackDamage);
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
            if (distanceToPlayer > monsterState.attackRange)
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
        
        if(health <= 0)
        {
            playerControl.isMonster = false;
            agent.isStopped = true;
            this.enabled = false;  // 스크립트 비활성화

            body.SetActive(false);
            dieParticle.Play();
            Invoke("Die",2.3f);
            return;
        }
        
        // 피격 상태로 전환
        StopAllCoroutines();
        StartCoroutine(HitRoutine());
    }
    private IEnumerator HitRoutine()
    {
        // 피격 시작
        isAttacking = true;  // 다른 행동 방지
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        
        animator.SetTrigger("Hit");
        ChangeColor(changeColor);
        
        // 피격 애니메이션 재생 시간
        yield return new WaitForSeconds(0.5f);
        
        // 색상 복구
        ChangeColor(basicColor);
        
        // 상태 복구
        isAttacking = false;
        agent.isStopped = false;
        
        // 플레이어가 여전히 범위 내에 있는지 확인
        if(player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= monsterState.attackRange)
            {
                currentState = State.Attacking;
            }
            else if (distanceToPlayer <= monsterState.wanderRadius)
            {
                currentState = State.Chasing;
            }
            else
            {
                currentState = State.Wandering;
            }
        }
        else
        {
            currentState = State.Wandering;
        }
    }

    private void Die()
    {
        
        // 사망 처리
        agent.enabled = false;
        gameObject.SetActive(false);
        //Destroy(gameObject, 2f);  // 2초 후 제거
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
        Gizmos.DrawWireSphere(transform.position, monsterState.wanderRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterState.attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward*monsterState.attackRange);
    }
    void PlayerCheck()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, monsterState.attackRange))
        {
            if (hit.collider.tag.Equals("Player"))
            {
                Debug.Log("you hitted");
                isplayer = true;
            }
            else {isplayer =false;}
        }
    }
    void ChangeColor(Color c)
    {
        Debug.Log("색변환");
        rendererColor.sharedMaterial.color = c;
    }
}