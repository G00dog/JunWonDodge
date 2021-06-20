using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public Text timeText;
    //public Text KillText;//적을 죽인 숫자 출력
    public GameObject bulletSpawnerPrefab;
    public GameObject itemPrefab;
    public GameObject level;

    public GameObject gameoverText;
    public Text recordText;
    public int KillCount = 0;//적을 죽인 숫자 세기
    public GameObject PlayerPos;

    int prevTime;
    //int spawnCounter = 0;
    private float surviveTime;
    private bool isGameover;
    private int enemy;

    bool isEvent = false;
    float eventTime;
    float eventCountTime = 0f;

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
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time : " + (int)surviveTime;
            enemy = (int)surviveTime / 15;

            int currTime = (int)(surviveTime % 5f);
            Debug.Log(prevTime + ", " + currTime);

            if (currTime == 0 && prevTime != currTime)
            {
                for (int i = 0; i <= enemy; i++)
                {
                    Vector3 randposBullet = PlayerPos.transform.position + new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
                    GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, randposBullet, Quaternion.identity);
                    bulletSpawner.transform.parent = level.transform;
                    bulletSpawner.transform.localPosition = randposBullet;

                    spawnerList.Add(bulletSpawner);
                }
               
                
                
                Vector3 randposItem = PlayerPos.transform.position + new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-8f, 8f));
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

                foreach(GameObject spawner in spawnerList)
                {
                    spawner.GetComponent<BulletSpawner>().isMoving = true;
                }
                isEvent = true;
                eventCountTime = 0f;
            }
            prevEventTime = eventTime;

            eventCountTime += Time.deltaTime;

            if (isEvent && eventCountTime > 5f)
            {
                eventCountTime = 0f;
                isEvent = false;

                foreach(GameObject spawner in spawnerList)
                {
                    spawner.GetComponent<BulletSpawner>().isMoving = false;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene2");
            }
        }
    }

    public void DieBulletSpanawner(GameObject spawner)
    {
        spawnerList.Remove(spawner);
    }

    public void EndGame()
    {
        isGameover = true;

        gameoverText.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;

            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        recordText.text = "Best Time : " + (int)bestTime;
    }
}
