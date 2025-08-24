using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private GameObject _upButton;
    private GameObject _leftButton;
    private GameObject _rightButton;
    private GameObject _downButton;
    private GameObject _backdrop;
    private GameObject _phantomButton;

    [SerializeField] float _spawnRate = 2f;
    [SerializeField] float _spawnCeiling = 5f;
    [SerializeField] float _fallingVelocity = 1f;
    [SerializeField] float _despawnFloor = -8f;


    private List<GameObject> _phantomButtons = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _backdrop = gameObject.transform.GetChild(0).gameObject;
        _upButton = gameObject.transform.GetChild(1).gameObject;
        _leftButton = gameObject.transform.GetChild(2).gameObject;
        _rightButton = gameObject.transform.GetChild(3).gameObject;
        _downButton = gameObject.transform.GetChild(4).gameObject;
        _phantomButton = gameObject.transform.GetChild(5).gameObject;

        InvokeRepeating("SpawnPhantomButtons", 2.0f, _spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = _phantomButtons.Count - 1; i >= 0; i--)
        {
            GameObject button = _phantomButtons[i];
            button.transform.position = button.transform.position + new Vector3(0, -1 * _fallingVelocity * Time.deltaTime, 0);
            if (button.transform.position.y <= _despawnFloor)
            {
                _phantomButtons.RemoveAt(i);
                Destroy(button);
            }
        }
    }

    void SpawnPhantomButtons()
    {
        int randomLane = UnityEngine.Random.Range(0, 4);
        GameObject spawnButton = gameObject.transform.GetChild(randomLane + 1).gameObject;
        float x = -1.875f + (1.25f * randomLane);
        GameObject newButton = Instantiate(spawnButton, new Vector3(x, _spawnCeiling, 0), Quaternion.identity, _phantomButton.transform);
        _phantomButtons.Add(newButton);
    }
}
