using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ShopStatistics : ScriptableObject
{
    public int meleeDamageUpgradeCount, rangedDamageUpgradeCount;
    public int maxHealthUpgradeCount;
    public int munitionUpgradeCount, maxMunitionUpgradeCount;
}
