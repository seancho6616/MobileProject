using System.Collections;
// using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;


public class MonsterController : MonoBehaviour
{
    // 몬스터 스탯
    public float maxHealth = 10f;
    public float moveSpeed = 5f;
    public float attackDamage = 4f;
    public float attackSpeed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 4f;

    // 현재 상태
    private float currentHealth;
    private Animator animator;
    private Transform player;
    private bool isPlayerInRange = false; // 플레이어가 범위 안에 있는지
    
    public enum MonsterState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Die
    }
    public MonsterState currentState;

    // 타이머들
    private float attackTimer;
    private float idleTimer;
    private Vector3 patrolPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentState = MonsterState.Idle;
    }

    // 플레이어가 감지 범위에 들어옴
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isPlayerInRange = true;
            
            // Idle이나 Patrol 상태면 바로 Chase로
            if (currentState == MonsterState.Idle || currentState == MonsterState.Patrol)
            {
                currentState = MonsterState.Chase;
                animator.SetBool("IsMoving", true);
            }
        }
    }

    // 플레이어가 감지 범위에서 나감
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
            
            // Chase 상태면 Idle로
            if (currentState == MonsterState.Chase)
            {
                currentState = MonsterState.Idle;
                animator.SetBool("IsMoving", false);
            }
        }
    }

    void Update()
    {
        // 현재 상태에 따라 행동
        switch (currentState)
        {
            case MonsterState.Idle:
                UpdateIdle();
                break;
            case MonsterState.Patrol:
                UpdatePatrol();
                break;
            case MonsterState.Chase:
                UpdateChase();
                break;
            case MonsterState.Attack:
                UpdateAttack();
                break;
        }
    }

    // Idle 상태 - 가만히 있다가 순찰
    void UpdateIdle()
    {
        idleTimer += Time.deltaTime;

        // 3초 후 순찰 시작
        if (idleTimer > 3f)
        {
            currentState = MonsterState.Patrol;
            SetRandomPatrolPoint();
            animator.SetBool("IsMoving", true);
            idleTimer = 0f;
        }
    }

    // Patrol 상태 - 랜덤하게 돌아다님
    void UpdatePatrol()
    {
        animator.SetBool("IsMoving", true);

        // 목표 지점으로 이동
        MoveTowards(patrolPoint);

        // 도착하면 다시 Idle
        if (Vector3.Distance(transform.position, patrolPoint) < 0.5f)
        {
            currentState = MonsterState.Idle;
            animator.SetBool("IsMoving", false);
            idleTimer = 0f;
        }
    }

    // Chase 상태 - 플레이어 쫓아감
    void UpdateChase()
    {
        // 플레이어가 범위 밖으로 나가면 이미 OnTriggerExit에서 처리됨
        if (player == null || !isPlayerInRange)
        {
            currentState = MonsterState.Idle;
            animator.SetBool("IsMoving", false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // 공격 범위 안이면 공격
        if (distance < attackRange)
        {
            currentState = MonsterState.Attack;
            animator.SetBool("IsMoving", false);
            return;
        }

        // 플레이어 쫓아가기
        MoveTowards(player.position);
    }

    // Attack 상태 - 플레이어 공격
    void UpdateAttack()
    {
        if (player == null || !isPlayerInRange)
        {
            currentState = MonsterState.Idle;
            animator.SetBool("IsMoving", false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // 멀어지면 다시 추적
        if (distance > attackRange)
        {
            currentState = MonsterState.Chase;
            animator.SetBool("IsMoving", true);
            return;
        }

        // 플레이어 바라보기
        LookAtPlayer();

        // 공격 쿨타임
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    // 목표 지점으로 이동
    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // 이동 방향 바라보기
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // 플레이어 바라보기
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // y축 회전 방지
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // 랜덤 순찰 지점 설정
    void SetRandomPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * 10f;
        randomDir.y = 0;
        patrolPoint = transform.position + randomDir;
    }

    // 공격
    void Attack()
    {
        animator.SetTrigger("Attack");
        
        // 플레이어에게 데미지
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < attackRange)
            {
                // 플레이어 체력 감소 (PlayerHealth 스크립트가 있다고 가정)
                // player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
                Debug.Log("플레이어 공격!");
            }
        }
    }

    // 데미지 받기
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 죽음
    void Die()
    {
        currentState = MonsterState.Die;
        animator.SetTrigger("Die");
        enabled = false; // Update 멈춤
        Destroy(gameObject, 2f); // 2초 후 삭제
    }

    // 디버그용 - 감지/공격 범위 표시
    void OnDrawGizmosSelected()
    {
        // 감지 범위 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 공격 범위 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}