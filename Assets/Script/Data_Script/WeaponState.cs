using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponState", menuName = "Scriptable Objects/WeaponState")]
public class WeaponState : ScriptableObject
{
    public enum Name {Stick,Umbrella, Brick, Nailwood, Pipe}
    public Name weaponType;
    public int id;

    public string weaponName;

    public float damage;

    public float attackSpeed;

    public float coinValue;
}
