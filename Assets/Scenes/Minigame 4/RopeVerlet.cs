using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RopeVerlet : MonoBehaviour
{
    [Header("Rope")]
    [SerializeField] private int numOfRopeSegments = 50;
    [SerializeField] private float ropeSegmentLength = 0.225f;

    [Header("Physics")]
    [SerializeField] private Vector2 gravityForce = new Vector2(0f, -2f);
    [SerializeField] private float dampingFactor = 0.98f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.1f;
    [SerializeField] private float bounceFactor = 0.1f;
    [SerializeField] private float correctionClampAmount = 0.1f;


    [Header("Constraints")]
    [SerializeField] private int numOfConstraintRuns = 50;

    [Header("Optimizations")]
    [SerializeField] private int collisionSegementInterval = 2;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private Vector3 ropeStartPosition;

    public struct RopeSegment
    {
        public Vector2 CurrentPosition;
        public Vector2 OldPosition;

        public RopeSegment(Vector2 pos)
        {
            CurrentPosition = pos;
            OldPosition = pos;
        }
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numOfRopeSegments;

        ropeStartPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        for (int i = 0; i < numOfRopeSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPosition));
            ropeStartPosition.y -= ropeSegmentLength;
        }
    }

    private void Update()
    {
        DrawRope();
    }

    private void DrawRope()
    {
        Vector3[] ropePositions = new Vector3[numOfRopeSegments];
        for (int i = 0; i < numOfRopeSegments; i++)
        {
            ropePositions[i] = ropeSegments[i].CurrentPosition;
        }

        lineRenderer.SetPositions(ropePositions);
    }

    private void FixedUpdate()
    {
        SimulateRope();

        for (int i = 0; i < numOfConstraintRuns; i++)
        {
            ApplyConstraints();

            if (i % collisionSegementInterval == 0)
            {
                HandleCollisions();
            }
        }
    }

    private void SimulateRope()
    {
        for (int i = 0; i < numOfRopeSegments; i++)
        {
            RopeSegment segment = ropeSegments[i];
            Vector2 velocity = (segment.CurrentPosition - segment.OldPosition) * dampingFactor;
            segment.OldPosition = segment.CurrentPosition;
            segment.CurrentPosition += velocity + (gravityForce * Time.fixedDeltaTime);
            ropeSegments[i] = segment;
        }
    }

    private void ApplyConstraints()
    {
        // First point always attached to Mouse Anchor.
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.CurrentPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < numOfRopeSegments - 1; i++)
        {
            RopeSegment currentSegment = ropeSegments[i];
            RopeSegment nextSegment = ropeSegments[i + 1];

            float distanceBetweenSegments = (currentSegment.CurrentPosition - nextSegment.CurrentPosition).magnitude;
            float difference = distanceBetweenSegments - ropeSegmentLength;

            Vector2 changeVector = (currentSegment.CurrentPosition - nextSegment.CurrentPosition).normalized * difference;

            if (i != 0)
            {
                currentSegment.CurrentPosition -= changeVector / 2f;
                nextSegment.CurrentPosition += changeVector / 2f;
            }
            else
            {
                nextSegment.CurrentPosition += changeVector;
            }

            ropeSegments[i] = currentSegment;
            ropeSegments[i + 1] = nextSegment;
        }
    }

    private void HandleCollisions()
    {
        for (int i = 1; i < numOfRopeSegments; i++)
        {
            RopeSegment segment = ropeSegments[i];
            Vector2 velocity = segment.CurrentPosition - segment.OldPosition;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(segment.CurrentPosition, collisionRadius, collisionMask);

            foreach (Collider2D collider in colliders)
            {
                Vector2 closestPoint = collider.ClosestPoint(segment.CurrentPosition);
                float distance = Vector2.Distance(segment.CurrentPosition, closestPoint);

                // if distance less than collision radius, resolve
                if (distance < collisionRadius)
                {
                    Vector2 normal = (segment.CurrentPosition - closestPoint).normalized;
                    if (normal == Vector2.zero)
                    {
                        normal = (segment.CurrentPosition - (Vector2)collider.transform.position).normalized;
                    }

                    float depth = collisionRadius - distance;
                    segment.CurrentPosition += normal * depth;

                    velocity = Vector2.Reflect(velocity, normal) * bounceFactor;
                }
            }

            segment.OldPosition = segment.CurrentPosition - velocity;
            ropeSegments[i] = segment;
        }
    }
}
