using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private TowerData towerData;
    public float speed = 8f;

    public void Launch(Enemy targetEnemy, TowerData data, Vector3 origin)
    {
        target = targetEnemy;
        towerData = data;
        transform.position = origin;
    }

    private void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            Hit();
        }
    }

    private void Hit()
    {
        if (towerData.isAoE)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.aoeRadius);
            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy == null) continue;
                enemy.TakeDamage(towerData.damage);

                if (towerData.appliesSlow)
                    enemy.ApplySlow(towerData.slowMultiplier, towerData.slowDuration);
            }
        }
        else
        {
            target.TakeDamage(towerData.damage);

            if (towerData.appliesSlow)
                target.ApplySlow(towerData.slowMultiplier, towerData.slowDuration);
        }

        Destroy(gameObject);
    }
}