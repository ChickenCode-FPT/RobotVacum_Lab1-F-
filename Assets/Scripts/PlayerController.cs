using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public List<SpriteRenderer> obstacleList;
    private Vector3 moveDirection;
    public float moveSpeed = 0.5f;
    private float timer;

    public Slider speedSlider; // ← Add this line

    private void Start()
    {
        RandomDirection();
    }

    public bool IsCollision(Vector3 minA, Vector3 maxA, Vector3 minB, Vector3 maxB)
    {
        return maxA.x > minB.x && minA.x < maxB.x && minA.y < maxB.y && maxA.y > minB.y;
    }

    void Update()
    {
        moveSpeed = speedSlider.value; // ← Update speed from slider
        MoveRandom();
    }

    void RandomDirection()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);
        moveDirection = new Vector3(x, y, 0).normalized;
    }

    void MoveRandom()
    {
        Vector3 nextDirection = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        bool collider = false;

        foreach (SpriteRenderer obstacle in obstacleList)
        {
            Bounds boundsA = GetComponent<SpriteRenderer>().bounds;
            float radius = Mathf.Min(boundsA.extents.x, boundsA.extents.y);
            boundsA.center = nextDirection;

            Bounds boundsB = obstacle.bounds;

            Vector3 minA = boundsA.min;
            Vector3 maxA = boundsA.max;
            Vector3 minB = boundsB.min;
            Vector3 maxB = boundsB.max;

            if (IsCircleAABB(new Vector2(nextDirection.x, nextDirection.y), radius, minB, maxB))
            {
                collider = true;
                break;
            }
        }

        if (!collider)
        {
            transform.position = nextDirection;
        }
        else
        {
            RandomDirection();
        }
    }

    bool IsCircleAABB(Vector2 centerCircle, float radius, Vector2 rectMin, Vector2 rectMax)
    {
        float closeX = Mathf.Clamp(centerCircle.x, rectMin.x, rectMax.x);
        float closeZ = Mathf.Clamp(centerCircle.y, rectMin.y, rectMax.y);

        float distX = Mathf.Abs(centerCircle.x - closeX);
        float distY = Mathf.Abs(centerCircle.y - closeZ);

        float distSqr = distX * distX + distY * distY;
        return distSqr <= radius * radius;
    }
}
