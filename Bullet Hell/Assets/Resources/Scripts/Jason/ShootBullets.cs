using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullets : MonoBehaviour
{
    private int attackPattern;

    [SerializeField] private float shootInterval = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private Transform startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = bulletSpawnPoint;
        attackPattern = Random.Range(0, 4);
        StartCoroutine(Shoot());
    }


    IEnumerable Shoot()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            switch (attackPattern)
            {
                case 0:
                    bullet.transform.position = 
                    break;
                case 1:
                    Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 45, 0));
                    break;
                case 2:
                    Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 90, 0));
                    break;
                case 3:
                    Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 135, 0));
                    break;
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
