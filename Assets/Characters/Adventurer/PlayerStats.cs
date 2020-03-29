using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public int money;
    public int meleeDamage, rangedDamage;
    public int maxHealth;
    public int munition, maxMunition;
    public float speed;
}
