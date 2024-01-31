using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public Color laserColor = Color.cyan;

    private Transform target;
    private float attackCooldown = 0f;

    private LineRenderer rangeCircle;
    private LineRenderer laserBeam;

    void Start()
    {
        CreateRangeCircle();
        CreateLaserBeam();
    }

    void Update()
    {
        if (target == null)
            FindTarget();

        if (target != null)
        {
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;
            }

            attackCooldown -= Time.deltaTime;
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        target = nearestEnemy?.transform;
    }

    void Attack()
    {
        if (target != null)
        {
            // Deal damage to the enemy
            Enemy enemyScript = target.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);

                // Show laser beam effect
                ShowLaserBeam();
            }
        }
    }

    void CreateRangeCircle()
    {
        rangeCircle = gameObject.AddComponent<LineRenderer>();
        rangeCircle.useWorldSpace = false;
        rangeCircle.positionCount = 360;
        rangeCircle.startWidth = 0.1f;
        rangeCircle.endWidth = 0.1f;

        UpdateRangeCircle();
    }

    void UpdateRangeCircle()
    {
        for (int i = 0; i < 360; i++)
        {
            float radian = i * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * attackRange;
            float y = Mathf.Sin(radian) * attackRange;
            rangeCircle.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    void CreateLaserBeam()
    {
        laserBeam = gameObject.AddComponent<LineRenderer>();
        laserBeam.useWorldSpace = true;
        laserBeam.startWidth = 0.1f;
        laserBeam.endWidth = 0.1f;
        laserBeam.material.color = laserColor;
        laserBeam.enabled = false;
    }

    void ShowLaserBeam()
    {
        if (target != null)
        {
            laserBeam.enabled = true;
            laserBeam.SetPosition(0, transform.position);
            laserBeam.SetPosition(1, target.position);
            Invoke("HideLaserBeam", 0.1f);
        }
    }

    void HideLaserBeam()
    {
        laserBeam.enabled = false;
    }
}
