using UnityEngine;
using System;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    public GameObject shieldPrefab; // Prefab for the shield
    public float shieldDuration = 5f; // Shield active time
    public float rotationSpeed = 100f; // Shield rotation speed
    public float shieldRadius = 2f; // Distance from the player
    public int shieldCount = 2; // Number of shield objects (180-degree coverage)
    public float shieldCooldown = 8f; // Time between shield activations

    private GameObject[] shields;
    private bool shieldActive = false;
    private bool canActivateShield = true; // To track cooldown status

    void Update()
    {
        // Activate shield on "Space" key press if it's not already active and cooldown has passed
        if (Input.GetKeyDown(KeyCode.Space) && !shieldActive && canActivateShield)
        {
            ActivateShield();
        }

        // Keep rotating the shield while active
        if (shieldActive)
        {
            RotateShields();
        }
    }

    void ActivateShield()
    {
        shieldActive = true;
        shields = new GameObject[shieldCount];

        // Spawn shield objects around the player
        for (int i = 0; i < shieldCount; i++)
        {
            float angle = i * 180f / (shieldCount - 1); // Spread across 180 degrees
            Vector3 shieldPosition = CalculateShieldPosition(angle);
            shields[i] = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity, transform);
        }

        StartCoroutine(ShieldDuration());
        StartCoroutine(ShieldCooldown()); // Start the cooldown timer
    }

    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(shieldDuration);
        DestroyShields();
        shieldActive = false;
    }

    void RotateShields()
    {
        for (int i = 0; i < shields.Length; i++)
        {
            if (shields[i] != null)
            {
                float angle = (i * 180f / (shieldCount - 1)) + (Time.time * rotationSpeed);

                // Use Math.PI for degrees-to-radians conversion
                float radians = angle * (float)(Math.PI / 180);
                float x = (float)Math.Cos(radians) * shieldRadius;
                float y = (float)Math.Sin(radians) * shieldRadius;

                shields[i].transform.position = transform.position + new Vector3(x, y, 0);
            }
        }
    }

    Vector3 CalculateShieldPosition(float angle)
    {
        // Use Math.PI for conversion instead of Unity methods
        float radians = angle * (float)(Math.PI / 180);
        float x = (float)Math.Cos(radians) * shieldRadius;
        float y = (float)Math.Sin(radians) * shieldRadius;

        return transform.position + new Vector3(x, y, 0);
    }

    void DestroyShields()
    {
        foreach (GameObject shield in shields)
        {
            if (shield != null)
            {
                Destroy(shield);
            }
        }
    }

    IEnumerator ShieldCooldown()
    {
        canActivateShield = false;
        yield return new WaitForSeconds(shieldCooldown); // Wait for cooldown duration
        canActivateShield = true; // Allow shield activation again
    }
}
