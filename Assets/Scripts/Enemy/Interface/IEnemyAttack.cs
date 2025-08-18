using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    void Attack(Transform enemy, Transform player);
}
