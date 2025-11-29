using UnityEngine;

[CreateAssetMenu(fileName = "MonsterState", menuName = "Scriptable Objects/MonsterState")]
public class MonsterState : ScriptableObject
{
    public enum Type { Slime1, Slime2 }
    public Type type;
    [Header("Basic Infomation")]
    public float heart;
    public float attackDamage;
    public float moveSpeed;

    [Header("Range")]
    public float wanderRadius;
    public float attackRange;

}
