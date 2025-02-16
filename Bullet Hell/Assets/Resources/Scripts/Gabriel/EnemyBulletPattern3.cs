using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBulletPattern3 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    public Transform blastPoint;
    public int blastCharge = 0;
    public int maxBlastCharge = 250;
    public float bulletsPerSecond = 10f;
    private Vector3 worldPoint;
    public float duration = 3f;
    public bool isShooting = false;
    private float currentAngle = 0;
    public float spread = 0;
    private bool blasting = false;
    public int amountOfProjectiles;
    public GameObject blastBar;
    private Image blastBarFill;
    private void Start()
    {
        StartCoroutine(ShootContinuously());
        blastBarFill = blastBar.transform.GetChild(0).GetComponent<Image>();
    }
    public void ToggleShooting(bool enable)
    {
        isShooting = enable;
    }
    public static Vector3 GetRandomScreenBorderPoint()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector3.zero;

        int border = Random.Range(0, 4);
        float x = 0, y = 0;

        if (border == 0)
        {
            x = Random.Range(0f, Screen.width);
            y = Screen.height;
        }
        else if (border == 1)
        {
            x = Random.Range(0f, Screen.width);
            y = 0;
        }
        else if (border == 2)
        {
            x = 0;
            y = Random.Range(0f, Screen.height);
        }
        else if (border == 3)
        {
            x = Screen.width;
            y = Random.Range(0f, Screen.height);
        }

        Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(x, y, cam.nearClipPlane + 1f));
        worldPoint.z = 0;
        return worldPoint;
    }
    private IEnumerator EaseIn(GameObject projectile)
    {
        Vector3 startPoint = projectile.transform.position;
        Vector3 endPoint = blastPoint.position;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            t = Mathf.Clamp01(t);
            t = t * t;

            if (projectile != null)
            {
                projectile.transform.position = Vector3.Lerp(startPoint, endPoint, t);
            }

            yield return null;
        }

        if (projectile != null)
        {
            projectile.transform.position = endPoint;
            Destroy(projectile);
        }
    }
    private void Shoot()
    {
        SpawnProjectile();
    }
    private void SpawnProjectile()
    {
        worldPoint = GetRandomScreenBorderPoint();
        GameObject projectile = Instantiate(projectilePrefab, worldPoint, blastPoint.rotation);
        StartCoroutine(EaseIn(projectile));
    }
    private void Shoot2()
    {
        for (int i = 0; i <= amountOfProjectiles; i++)
        {
            SpawnProjectile2(currentAngle);
            currentAngle += (360 / amountOfProjectiles);
        }
        currentAngle += spread;
    }
    private void SpawnProjectile2(float angle)
    {
        GameObject projectile = Instantiate(projectilePrefab2, blastPoint.position, blastPoint.rotation);
        projectile.transform.Rotate(0, 0, angle);
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            if (isShooting && blasting == false)
            {
                Shoot();
                blastCharge++;
                UpdateBlastBar();
            }

            if (blastCharge >= maxBlastCharge)
            {
                blasting = true;
            }

            if (blasting == true)
            {
                Shoot2();
                blastCharge--;
                UpdateBlastBar();
                if (blastCharge == 0)
                {
                    blasting = false;
                    yield return new WaitForSeconds(1);
                }
            }
            yield return new WaitForSeconds(1f / bulletsPerSecond);
        }
    }
    private void UpdateBlastBar()
    {
        float blastPercent = (float)blastCharge / maxBlastCharge;
        blastBarFill.fillAmount = blastPercent;
    }
}
