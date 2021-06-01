using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public Text timeText;
    public GameObject bulletSpawnerPrefab;
    public GameObject itemPrefab;
    public GameObject level;
    int prevTime;
    int spawnCounter = 0;
    private float surviveTime;
    private bool isGameover;

    int prevEventTime;

    List<GameObject> itemList = new List<GameObject>();
    List<GameObject> spawnerList = new List<GameObject>();

    void Start()
    {
        surviveTime = 0;
        isGameover = false;
        prevTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time: " + (int)surviveTime;

            int currTime = (int)(surviveTime % 5f);
            Debug.Log(prevTime + ", " + currTime);

            if (currTime == 0 && prevTime != currTime)
            {
                Vector3 randposBullet = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-8f, 8f));
                GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, randposBullet, Quaternion.identity);
                bulletSpawner.transform.parent = level.transform;
                bulletSpawner.transform.localPosition = randposBullet;

                spawnerList.Add(bulletSpawner);

                Vector3 randposItem = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-8f, 8f));
                GameObject item = Instantiate(itemPrefab, randposItem, Quaternion.identity);
                item.transform.parent = level.transform;
                item.transform.localPosition = randposItem;

                itemList.Add(item);
            }

            prevTime = currTime;

            int eventTime = (int)(surviveTime % 10f);
            if (eventTime == 0 && prevEventTime != eventTime)
            {
                foreach(GameObject item in itemList)
                {
                    Destroy(item);
                }
                itemList.Clear();
            }
            prevEventTime = eventTime;
        }
    }
}
