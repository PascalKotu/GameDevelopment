using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ShopStatistics : ScriptableObject
{
    [SerializeField] public int meleeDamageUpgradeCount, rangedDamageUpgradeCount;
    [SerializeField] public int maxHealthUpgradeCount;
    [SerializeField] public int munitionUpgradeCount, maxMunitionUpgradeCount;
}
