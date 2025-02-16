using UnityEngine;
using System;
using System.Collections;

public class J_PlayerShield : MonoBehaviour
{
    public GameObject shieldPrefab;
    public float shieldDuration = 5f;
    public float rotationSpeed = 100f;
    public float shieldRadius = 2f;
    public int shieldCount = 2;
    public float shieldCooldown = 8f;

    private GameObject[] shields;
    private bool shieldActive = false;
    private bool canActivateShield = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !shieldActive && canActivateShield)
        {
            ActivateShield();
        }

        if (shieldActive)
        {
            RotateShields();
        }
    }

    void ActivateShield()
    {
        shieldActive = true;
        shields = new GameObject[shieldCount];

        for (int i = 0; i < shieldCount; i++)
        {
            float angle = i * 180f / (shieldCount - 1);
            Vector3 shieldPosition = CalculateShieldPosition(angle);
            shields[i] = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity, transform);
        }

        StartCoroutine(ShieldDuration());
        StartCoroutine(ShieldCooldown());
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
                // Calculate the current angle for each shield, adding rotation over time.
                float angle = (i * 180f / (shieldCount - 1)) + (Time.time * rotationSpeed);

                // Convert the angle to radians
                float radians = angle * (float)(Math.PI / 180);

                // Calculate the x and y position using cos and sin.
                float x = (float)Math.Cos(radians) * shieldRadius;
                float y = (float)Math.Sin(radians) * shieldRadius;

                // Update the shield position relative to the player.
                shields[i].transform.position = transform.position + new Vector3(x, y, 0);
            }
        }
    }

    Vector3 CalculateShieldPosition(float angle)
    {
        // Convert the angle to radians.
        float radians = angle * (float)(Math.PI / 180);

        // Calculate the x and y position using cos and sin.
        float x = (float)Math.Cos(radians) * shieldRadius;
        float y = (float)Math.Sin(radians) * shieldRadius;
        
        // Return the calculated position relative to the player.
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
        yield return new WaitForSeconds(shieldCooldown);
        canActivateShield = true;
    }
}