using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<EnemyHP>())
        {
            EnemyHP enemyHP = other.gameObject.GetComponent<EnemyHP>();
            enemyHP.TakeDamage(damageAmount);
        }
    }
}