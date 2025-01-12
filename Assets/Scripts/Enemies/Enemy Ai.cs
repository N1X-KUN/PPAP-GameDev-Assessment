using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType; // Attach script that implements IEnemy
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking,
        Tracking // Added state for tracking behavior
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyFound enemyFound;

    private void Awake()
    {
        enemyFound = GetComponent<EnemyFound>();
        state = State.Roaming; // Default to roaming
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;

            case State.Tracking: // Added tracking behavior
                Tracking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyFound.MoveTo(roamPosition);

        // Check if player is within attack range
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
        {
            state = enemyType != null ? State.Attacking : State.Tracking; // Switch to appropriate state
        }

        // Change roaming position after a certain time
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
            return;
        }

        if (canAttack)
        {
            canAttack = false;

            // Use fallback behavior if enemyType is invalid
            if (enemyType is IEnemy enemy)
            {
                enemy.Attack();
            }
            else
            {
                Debug.LogWarning($"EnemyType is not set or doesn't implement IEnemy on {gameObject.name}. Using fallback behavior.");
                PerformDefaultAttack();
            }

            if (stopMovingWhileAttacking)
            {
                enemyFound.StopMoving();
            }
            else
            {
                enemyFound.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private void PerformDefaultAttack()
    {
        Debug.Log($"{gameObject.name} performs a default attack!");
        // Add visual effects or other fallback actions here
    }


    private void Tracking()
    {
        if (Player.Instance == null)
        {
            enemyFound.StopMoving();
            return;
        }

        // Move towards the player (tracking behavior)
        Vector2 moveDirection = (Player.Instance.transform.position - transform.position).normalized;
        enemyFound.MoveTo(moveDirection);

        // If within attack range and `enemyType` is set, switch to attacking
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange && enemyType != null)
        {
            state = State.Attacking;
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
