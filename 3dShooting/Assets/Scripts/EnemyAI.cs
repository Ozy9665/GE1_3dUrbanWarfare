using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 5f;
    public float hp = 100f;
    public Transform player;
    public GameObject bullet;
    public Transform bulletPos;
    public float fireRate = 2f;

    private float fireTimer;

    public GameObject impactEffectPrefab;
    public float explosionForce = 100f;
    public float explosionRadius = 5f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if(fireTimer<=0f)
        {
            Shot();
            fireTimer = fireRate;
        }
    }

    void Shot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.up * -10;
    }

    void InstantiateImpactEffect()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject cube = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = cube.AddComponent<Rigidbody>();
            Vector3 randomDirection = Random.insideUnitSphere * explosionRadius;
            rb.AddForce(randomDirection * explosionForce);
            Destroy(cube, 2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            InstantiateImpactEffect();
            Destroy(gameObject);
        }
    }
}
