using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    void Start()
    {
        float newX = Random.Range(3.6f, -3.6f);
        var transformLocalPosition = transform.localPosition;
        transformLocalPosition.x = newX;
    }
}
