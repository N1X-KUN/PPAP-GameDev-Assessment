using UnityEngine;

public class Tracker : MonoBehaviour
{
    private EnemyFound enemyFound;

    private void Awake()
    {
        enemyFound = GetComponent<EnemyFound>();
    }

    private void Update()
    {
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        if (Player.Instance == null)
        {
            enemyFound.StopMoving();
            return;
        }

        // Move towards the player
        Vector2 moveDirection = (Player.Instance.transform.position - transform.position).normalized;
        enemyFound.MoveTo(moveDirection);
    }
}
