using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;

    public int hp = 150;
    public HPBar hpbar;

    public AudioSource audioSource;

    public bool isMoving = false;
    private NavMeshAgent nvAgent;
    Animator animator;

    void Start()
    {
        timeAfterSpawn = 0f;

        spawnRate = Random.Range(spawnRateMin, spawnRateMax);

        target = FindObjectOfType<PlayerController>().transform;

        StartCoroutine(MonsterAI());

        nvAgent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            return;
        }

        timeAfterSpawn += Time.deltaTime;

        transform.LookAt(target);

        if (timeAfterSpawn >= spawnRate)
        {
            audioSource.Play();

            timeAfterSpawn = 0f;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

            bullet.transform.LookAt(target);

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }

    IEnumerator MonsterAI()
    {
        while(hp>0)
        {
            yield return new WaitForSeconds(0.2f);

            if(isMoving)
            {
                nvAgent.destination = target.position;
                nvAgent.isStopped = false;
                animator.SetBool("isMoving", true);
            }
            else
            {
                nvAgent.isStopped = true;
                animator.SetBool("isMoving", false);
            }
        }
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        hpbar.SetHP(hp);
        EffectManager.PlayEffect(transform.position);

        if (hp <= 0)
        {
            Scream Sound = FindObjectOfType<Scream>();
            Sound.scream();

            animator.SetTrigger("Die");

            GameManager2 gameManager = FindObjectOfType<GameManager2>();

            gameManager.DieBulletSpanawner(gameObject);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //gameManager.KillCount++;
            Destroy(gameObject, 5f);
        }
    }
}
