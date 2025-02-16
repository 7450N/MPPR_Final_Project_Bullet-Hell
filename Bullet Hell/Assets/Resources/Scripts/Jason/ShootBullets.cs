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


    IEnumerator Shoot()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            float bulletDir = Random.Range(0, 1);

            
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
