using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [Header("Move peed")]
    public float moveSpeed = 5f;

    bool canDash = true;
    bool canAttack = true;
    bool canMove = true;
    

    Vector3 movement;
    Rigidbody rigid;
    Animator ani;

    PlayerStemina playerStemina;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        playerStemina = GetComponent<PlayerStemina>();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move() // 이동
    {
        Vector3 move = rigid.position + movement * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(move);
    }
    void OnMovement(InputValue value) // 이동 버튼 입력
    {
        if(!canMove) return;
        Vector2 input = value.Get<Vector2>();
        if(input != null)
        {
            movement = new Vector3(input.x, 0f, input.y);
            bool isMoving = input.magnitude > 0.1f; // 데드존 고려
        
        // 애니메이터에 상태 전달
            ani.SetBool("IsMove", isMoving);    
        }
    }

    IEnumerator OnAttack(){ //공격 버튼 입력
        if(!canAttack) yield break;
        canMove = false;
        ani.SetBool("IsMove", false);
        movement = Vector3.zero;
        ani.SetTrigger("Attack1");
        yield return new WaitForSeconds(.5f);
        canMove = true;
    }
    IEnumerator OnDash() // 대시 버튼 입력
    {   
        bool trueFalse = playerStemina.UseStamina(25);
        if(!canDash || !trueFalse) yield break;
        (canDash, canMove, canAttack) = (false, false, false);
        ani.SetTrigger("Dash");
        Vector3 move;
        if(movement == Vector3.zero)
        {
            move = Vector3.forward *8f;
        }
        else
        {
            move = movement*5f;
        }
        rigid.AddForce(move, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canAttack = true;        
        yield return new WaitForSeconds(1.0f);
        canDash = true;
    }
}
