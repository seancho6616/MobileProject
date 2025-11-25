using System;
[Serializable]
public class GameData
{
    public string lastScene;
    public PositionData position; // 위치 - x, y, z 좌표

    public int coins;
    public int potionCount;

    public int maxHeart;
    public int currentHeart;
    public int maxStamina;
    public int currentStamina;
    public int speed;
    public int baseAttack;
    public int baseAttackSpeed;
    public int attackRange;

    public int equippedWeaponId; // 무기는 ID만 저장
}

// 좌표 저장
[Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;

    public PositionData(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
}