using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public GameObject Item;
    [Header("Move peed")]
    public float moveSpeed = 5f;

    bool canDash = true;
    bool canAttack = true;
    bool canMove = true;

    public bool pickupPotion = false;
    public bool pickupHeart = false;
    public bool pickupDisHeart = false;
    public bool pickupStamina = false;
    
    Vector3 movement;
    Rigidbody rigid;
    Animator ani;
    PlayerStemina playerStemina;
    HeartManager heartManager;
    PlayerStats playerStats;
    TextUI textUI;
    AttackImageChanger attackImageChanger;
    Renderer[] partsRenderers;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        playerStemina = GetComponent<PlayerStemina>();
        heartManager = GetComponent<HeartManager>();
        playerStats = GetComponent<PlayerStats>();
        textUI = FindAnyObjectByType<TextUI>();
        attackImageChanger = FindAnyObjectByType<AttackImageChanger>();
        partsRenderers = GetComponentsInChildren<Renderer>(true);
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
        if (pickupHeart)
        {
            movement = Vector3.zero;
            ani.SetTrigger("Action");
            playerStats.MaxHealth =4f;
            heartManager.MakeSameHeart();
            pickupHeart = false;
            attackImageChanger.BeforeChangeSprite();
            Destroy(Item);
        }
        else if (pickupStamina)
        {
            movement = Vector3.zero;

            ani.SetTrigger("Action");
            if(playerStemina.MaxStamina > playerStemina.currentStamina)
            {
                playerStemina.currentStamina += 25f;
                playerStemina.MaxStamina = 25f;    
                playerStemina.UpdateStamina();
            }
            pickupStamina = false;
            attackImageChanger.BeforeChangeSprite();
            Destroy(Item);
        }
        else if (pickupPotion)
        {
            movement = Vector3.zero;

            ani.SetTrigger("Action");
            int count = playerStats.PotionCount += 1;
            textUI.CountPotion(count);
            pickupPotion = false;
            attackImageChanger.BeforeChangeSprite();
            Destroy(Item);
        }
        else if(canAttack)
        {
            movement = Vector3.zero;
            ani.SetTrigger("Attack1");
        }
        yield return new WaitForSeconds(.5f);
        canMove = true;
    }
    IEnumerator OnDash() // 대시 버튼 입력
    {   
        if(!canDash) yield break;
        bool trueFalse = playerStemina.UseStamina(25);
        if(!trueFalse) yield break;
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
    IEnumerator OnHeal()
    {
        int count = playerStats.PotionCount;
        if(count == 0) yield break;
        (canDash, canMove, canAttack) = (false, false, false);
        playerStats.healParticle.Play();
        playerStats.CurrentHealth = 4f;
        count = playerStats.PotionCount -= 1;
        textUI.CountPotion(count);
        yield return new WaitForSeconds(1.0f);
        (canDash, canMove, canAttack) = (true, true, true);

    }
    Color damageColor = new Color(255f,78f,87f,123f);
    Color basicColor = new Color(182f,169f,163f,255f);
    public void Damaged(float value)
    {
        playerStats.CurrentHealth = -value;
        // ChangeColor(damageColor);
        //yield return new WaitForSeconds(.1f);
        // ChangeColor(basicColor);
        // yield break;
    }

    void ChangeColor(Color newColor)
    {
        foreach (Renderer renderer in partsRenderers)
        {
            renderer.material.color = newColor;
        }
    }


    public void MakeFalse()
    {
        pickupDisHeart = false;
        pickupHeart = false;
        pickupPotion = false;
        pickupStamina = false;
    }
}
