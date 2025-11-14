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
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        Vector3 move = rigid.position + movement * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(move);
    }
    void OnMovement(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if(input != null)
        {
            movement = new Vector3(input.x, 0f, input.y);
            bool isMoving = input.magnitude > 0.1f; // 데드존 고려
        
        // 애니메이터에 상태 전달
            ani.SetBool("IsMove", isMoving);    
            Debug.Log($"Move : {input}");
            
        }
    }

    void OnAttack(){
        if(!canAttack) return;
        ani.SetTrigger("Attack1");
    }
    IEnumerator OnDash()
    {   
        if(!canDash) yield break;
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
