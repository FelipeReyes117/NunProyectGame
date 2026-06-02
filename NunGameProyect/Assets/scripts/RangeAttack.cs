using UnityEngine;

public class RangedAttack : EnemyAttackBase
{
    [Header("Ranged")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public Transform firePoint;

    protected override void Attack()
    {
        if (projectilePrefab == null) return;

    Vector3 origin = firePoint != null ? firePoint.position : transform.position;
    Vector2 direction = (player.position - origin).normalized;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    GameObject proj = Instantiate(
        projectilePrefab,
        origin,
        Quaternion.Euler(0f, 0f, angle)
    );
    BulletEnemyScript bullet = proj.GetComponent<BulletEnemyScript>();
    if (bullet != null)
    {
        Collider2D ownerCollider = GetComponent<Collider2D>();
        bullet.SetOwner(ownerCollider);
    }

    Destroy(proj, 4f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}