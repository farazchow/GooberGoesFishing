using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Minigame4Controller : MonoBehaviour
{
    public GameObject predatorFish;
    public float scrollSpeed = 5f;
    public float deletionFloor = -10f;
    public float spawnCeiling = 10f;
    public float spawnFrequency = 2f;

    private List<GameObject> obstacles;
    private float timeElapsed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= spawnFrequency)
        {
            timeElapsed %= spawnFrequency;
            GameObject newFish = Instantiate(predatorFish, new Vector3(0, spawnCeiling, 0), Quaternion.identity);
            obstacles.Add(newFish);
        }

        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = obstacles[i];
            obstacle.transform.position = obstacle.transform.position + new Vector3(0, -scrollSpeed * Time.deltaTime, 0);

            if (obstacle.transform.position.y <= deletionFloor)
            {
                obstacles.RemoveAt(i);
                Destroy(obstacle);
            }
        }
    }
}
