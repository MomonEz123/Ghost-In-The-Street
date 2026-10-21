using UnityEngine;

public class DirectionalWeaponPrefab : MonoBehaviour
{
    private DirectionalWeapon weapon;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float duration;

    // Centang jika sprite proyektil ingin diputar menghadap arah tembakan (sprite default menghadap kanan)
    [SerializeField] private bool rotateToDirection = false;

    void Start()
    {
        weapon = GameObject.Find("Directional Weapon").GetComponent<DirectionalWeapon>();
        direction = GetMouseDirection();
        if (rotateToDirection)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        duration = weapon.stats[weapon.weaponLevel].duration;
        rb = GetComponent<Rigidbody2D>();
        float randomAngle = Random.Range(-0.2f, 0.2f);
        rb.linearVelocity = new Vector3(
            direction.x * weapon.stats[weapon.weaponLevel].speed + randomAngle, direction.y * weapon.stats[weapon.weaponLevel].speed + randomAngle);
        //Destroy(gameObject, weapon.stats[weapon.weaponLevel].duration);
        AudioController.Instance.PlaySound(AudioController.Instance.directionalWeaponSpawn);
    }


    // Arah dari posisi tembak ke kursor mouse (world space)
    private Vector3 GetMouseDirection()
    {
        Camera cam = Camera.main;
        if (cam == null)
            return PlayerController.Instance.lastMoveDirection;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorld - transform.position;
        dir.z = 0f;

        // Kursor tepat di atas player: pakai arah gerak terakhir
        if (dir.sqrMagnitude < 0.0001f)
            return PlayerController.Instance.lastMoveDirection;

        return dir.normalized;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * 5);
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            //AudioController.Instance.PlaySound(AudioController.Instance.directionalWeaponHit);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Abaikan collision dengan Player
        if (collision.gameObject.CompareTag("Player"))
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
                AudioController.Instance.PlaySound(AudioController.Instance.directionalWeaponHit);
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
                // AudioController.Instance.PlaySound(AudioController.Instance.directionalWeaponHit);
            }
        }
    }*/
}
