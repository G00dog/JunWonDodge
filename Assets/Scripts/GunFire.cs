using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    private float spawnRate = 0.5f;
    private float timerAfterSpawn;
    public GameObject playerbulletPrefab;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timerAfterSpawn += Time.deltaTime;

        if (Input.GetButton("Fire1") && timerAfterSpawn >= spawnRate)
        {
            timerAfterSpawn = 0;
            GameObject bullet = Instantiate(playerbulletPrefab, transform.position, transform.rotation);
            audioSource.Play();
        }
    }
}
