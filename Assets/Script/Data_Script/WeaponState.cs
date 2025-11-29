using UnityEngine;

[CreateAssetMenu(fileName = "WeaponState", menuName = "Scriptable Objects/WeaponState")]
public class WeaponState : ScriptableObject
{
    public enum Name {Stick, Nailwood, Pipe, Brick}
    public Name weaponType;

    public string weaponName;

    public float Damage;

    public float attackSpeed;

    public float coinValue;
}
