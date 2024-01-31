using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public float maxHealth = 100f;
    public float timeBetweenSpawns = 2f;

    public GameObject enemyPrefab;  // Assign your Enemy prefab in the Unity Editor

    private float currentHealth;
    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        waypoints = WaypointManager.Instance.waypoints;

        if (waypoints.Count > 0)
        {
            Transform firstWaypoint = waypoints[0];
            transform.position = firstWaypoint.position;
        }

        // Start the spawn coroutine
        StartCoroutine(SpawnEnemiesWithTimer());
    }

    // Coroutine for spawning enemies with a new timer
    IEnumerator SpawnEnemiesWithTimer()
    {
        while (currentWaypointIndex < waypoints.Count)
        {
            // Spawn a new enemy prefab
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Set the new enemy's waypoints and start moving
            Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
            newEnemyScript.currentWaypointIndex = currentWaypointIndex;

            // Move to the next waypoint in the original enemy
            currentWaypointIndex++;

            // Wait for the specified time before spawning the next enemy
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Reached the end of the path
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce the enemy's health when it takes damage
        currentHealth -= damage;

        // Check if the enemy has been defeated
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Perform actions when the enemy is defeated (e.g., play death animation, award points, etc.)
        Destroy(gameObject);
    }
}
