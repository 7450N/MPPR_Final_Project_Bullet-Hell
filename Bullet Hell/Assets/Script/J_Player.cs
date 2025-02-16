using UnityEngine;

public class J_Player : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 screenBounds;
    public float playerRadius = 0.2f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private Vector2 position;
    private Vector2 bulletDirection = Vector2.up;
    public float fireRate = 0.2f;
    private float fireCooldown = 0f;

    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.y);

        Camera mainCamera = Camera.main;
        Vector3 screenBottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector3 screenTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        // Calculate the screen bounds by finding the half-width and half-height
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

        if (Input.GetKey("w")) moveY = 1;
        if (Input.GetKey("s")) moveY = -1;
        if (Input.GetKey("a")) moveX = -1;
        if (Input.GetKey("d")) moveX = 1;

        // Update player position based on input and speed.
        position += new Vector2(moveX, moveY) * speed * Time.deltaTime;

        // Clamps the player's position within the screen bounds.
        position.x = Clamp(position.x, -screenBounds.x, screenBounds.x);
        position.y = Clamp(position.y, -screenBounds.y, screenBounds.y);

        transform.position = new Vector3(position.x, position.y, 0);
    }

    void FireBullet()
    {
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, position + new Vector2(0, 0.5f), Quaternion.identity);

            J_PlayerBullet bullet = bulletObject.GetComponent<J_PlayerBullet>();

            // Initialize the bullet with the player's position and direction.
            bullet.Initialize(position + new Vector2(0, 0.5f), bulletDirection);

            fireCooldown = fireRate;
        }

        if (fireCooldown > 0f)
        {
            // Decrease the cooldown over time.
            fireCooldown -= Time.deltaTime;
        }
    }

    private float Clamp(float value, float min, float max)
    {
        // Clampz the value to the specified minimum and maximum range.
        return value < min ? min : (value > max ? max : value);
    }
}
