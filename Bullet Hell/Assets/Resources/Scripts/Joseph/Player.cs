using UnityEngine;


public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 screenBounds;
    public float playerRadius = 0.2f;  // Adjusted for player scale
    public GameObject bulletPrefab;    // Bullet prefab to instantiate
    public float bulletSpeed = 10f;    // Bullet speed

    private Vector2 position;  // Player's position
    private Vector2 bulletDirection = Vector2.up;  // Direction of the bullet (upwards)
    public float fireRate = 0.2f;  // Time between each bullet fired
    private float fireCooldown = 0f;  // Cooldown to manage firing rate

    void Start()
    {
        // Initial player position
        position = new Vector2(transform.position.x, transform.position.y);

        // Calculate screen bounds
        Camera mainCamera = Camera.main;
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        screenBounds = new Vector2(
            Mathf.Abs(screenTopRight.x - screenBottomLeft.x) / 2,
            Mathf.Abs(screenTopRight.y - screenBottomLeft.y) / 2
        );
    }

    void Update()
    {
        HandleMovement();
        FireBullet();
    }

    void HandleMovement()
    {
        float moveX = 0;
        float moveY = 0;

        // Move the player using arrow keys or WASD
        if (Input.GetKey("w")) moveY = 1;
        if (Input.GetKey("s")) moveY = -1;
        if (Input.GetKey("a")) moveX = -1;
        if (Input.GetKey("d")) moveX = 1;

        position += new Vector2(moveX, moveY) * speed * Time.deltaTime;

        // Clamp player within screen bounds
        position.x = Clamp(position.x, -screenBounds.x, screenBounds.x);
        position.y = Clamp(position.y, -screenBounds.y, screenBounds.y);

        transform.position = new Vector3(position.x, position.y, 0);
    }

    void FireBullet()
    {
        // Check if left mouse button is held down and cooldown allows firing
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            // Instantiate the bullet prefab
            GameObject bulletObject = Instantiate(bulletPrefab, position + new Vector2(0, 0.5f), Quaternion.identity);

            // Get the PlayerBullet script from the bullet prefab and initialize it
            PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();
            bullet.Initialize(position + new Vector2(0, 0.5f), bulletDirection);

            // Reset the fire cooldown
            fireCooldown = fireRate;
        }

        // Update the fire cooldown
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    private float Clamp(float value, float min, float max)
    {
        return value < min ? min : (value > max ? max : value);
    }
}
