using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [SerializeField] public int money;
    [SerializeField] public int meleeDamage, rangedDamage;
    [SerializeField] public int maxHealth;
    [SerializeField] public int munition, maxMunition;
}
