using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PredatorFish : MonoBehaviour
{
    public float xMax;
    public float xMin;
    public float speed = 1f;
    private SpriteRenderer sprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        if (Random.Range(0, 2) == 0)
        {
            sprite.flipX = true;
            speed *= -1;
        }
        gameObject.transform.position = new Vector3(Random.Range(xMin, xMax), gameObject.transform.position.y, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x >= xMax)
        {
            gameObject.transform.position = new Vector3(xMax, gameObject.transform.position.y, gameObject.transform.position.z);
            sprite.flipX = !sprite.flipX;
            speed *= -1;
        }
        if (gameObject.transform.position.x <= xMin)
        {
            gameObject.transform.position = new Vector3(xMin, gameObject.transform.position.y, gameObject.transform.position.z);
            sprite.flipX = !sprite.flipX;
            speed *= -1;
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + Time.deltaTime * speed, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
