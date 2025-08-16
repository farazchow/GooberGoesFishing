using System.Collections.Generic;
using UnityEngine;

public class RopeHinges : MonoBehaviour
{
    [Header("Rope Segments")]
    [SerializeField] private float _segmentLength = .05f;
    [SerializeField] private float _segmentWidth = .01f;
    [SerializeField] private int _numberOfSegments = 20;
    [SerializeField] private GameObject _ropeSegment;
    private List<GameObject> _ropeSegments = new List<GameObject>();
    private GameObject _anchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 startPosition = gameObject.transform.position;
        for (int i = 0; i < _numberOfSegments; i++)
        {
            GameObject newSegment = Instantiate(_ropeSegment, startPosition, Quaternion.identity, gameObject.transform);
            newSegment.transform.localScale = new Vector3(_segmentWidth, _segmentLength, 1);
            if (i == 0)
            {
                newSegment.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
            }
            else
            {
                newSegment.GetComponent<HingeJoint2D>().connectedBody = _ropeSegments[i - 1].GetComponent<Rigidbody2D>();
            }
            _ropeSegments.Add(newSegment);
            startPosition.y -= _segmentLength;
        }

        _anchor = Instantiate(new GameObject("Anchor"), _ropeSegments[0].transform.localPosition, Quaternion.identity, gameObject.transform);
        _anchor.transform.localScale = new Vector3(_segmentWidth, _segmentLength, 1);
        _anchor.AddComponent<Rigidbody2D>();
        _anchor.AddComponent<HingeJoint2D>().anchor = _ropeSegments[0].GetComponent<HingeJoint2D>().anchor;
        _ropeSegments[0].GetComponent<HingeJoint2D>().connectedBody = _anchor.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
