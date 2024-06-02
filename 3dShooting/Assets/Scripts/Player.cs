using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Camera followCamera;
    public float rate;
    public Transform bulletPos;
    public GameObject bullet;

    float hAxis;
    float vAxis;

    bool fDown; //°ø°Ý
    bool isSide;// º®
    bool isFireReady = true;
    bool isDead;

    Vector3 moveVec;
    Vector3 sideVec;

    float fireDelay;


    public GameObject impactEffectPrefab;
    public float explosionForce = 100f;
    public float explosionRadius = 5f;

    // Start is called before the first frame update
    void Start()
    {
        sideVec = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Attack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        fDown = Input.GetButton("Fire");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isSide && moveVec == sideVec)
            moveVec = Vector3.zero;

        transform.position += moveVec * speed * Time.deltaTime;
    }

    void Turn()
    {
        if(moveVec != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.5f);
        }
    }

    void Attack()
    {
        fireDelay += Time.deltaTime;
        isFireReady = rate < fireDelay;

        if(fDown && isFireReady && !isDead)
        {
            StartCoroutine("Shot");
            fireDelay = 0;
        }
    }

    IEnumerator Shot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.up * -50;

        yield return null;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyBullet"))
        {
            InstantiateImpactEffect();
            Destroy(gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("stay");
            isSide = true;
            sideVec = moveVec;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("exit");
            isSide = false;
            sideVec = Vector3.zero;
        }
    }

    void InstantiateImpactEffect()
    {
        for(int i = 0; i < 100; i++)
        {
            GameObject cube = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = cube.AddComponent<Rigidbody>();
            Vector3 randomDirection = Random.insideUnitSphere * explosionRadius;
            rb.AddForce(randomDirection * explosionForce);
            Destroy(cube, 2f);
        }
    }
}
