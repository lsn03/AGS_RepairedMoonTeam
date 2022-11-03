using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField]
    Transform center;

    [SerializeField]
    float radius = 2f, angularSpeed = 2f;

    float positionX, positionY, angle = 0f;

    void Update()
    {
        positionX = center.position.x + Mathf.Cos(angle) * radius;
        positionY = center.position.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector2(positionX, positionY);

        angle += Time.deltaTime * angularSpeed;

        if (angle >= 360f)
        {
            angle -= 360f;
        }
    }

}
