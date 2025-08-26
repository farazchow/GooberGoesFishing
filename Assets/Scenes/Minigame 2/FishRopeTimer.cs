using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FishRopeTimer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Transform _fish;
    [SerializeField] private int _numOfRopePoints = 50;
    [SerializeField] private float _amplitude;
    [SerializeField] private bool flip = false;

    [SerializeField] private float _width = 5f;
    private float _elapsedTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _numOfRopePoints;
        _fish = gameObject.transform.GetChild(0);
        SpriteRenderer fishRenderer = _fish.GetComponent<SpriteRenderer>();
        fishRenderer.enabled = true;
        if (flip)
        {
            fishRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        DrawFishRope();
    }

    private void DrawFishRope()
    {
        Vector3[] drawPoints = new Vector3[_numOfRopePoints];
        float startingXPos = -1 * (_width / 2);
        float increment = _width / _numOfRopePoints;

        if (flip)
        {
            startingXPos *= -1;
            increment *= -1;
        }

        for (int i = 0; i < _numOfRopePoints; i++)
        {
            float x = (increment * i) + startingXPos;
            float y = _amplitude * Mathf.Sin(x + _elapsedTime) + transform.position.y;
            drawPoints[i] = new Vector3(x, y, 0);
        }

        _lineRenderer.SetPositions(drawPoints);

        _fish.position = drawPoints[0] + new Vector3(0, 0, -1);
        Vector3 tailVector = drawPoints[1] - drawPoints[0];
        _fish.rotation = Quaternion.FromToRotation(Vector3.right, tailVector);
    }
}
