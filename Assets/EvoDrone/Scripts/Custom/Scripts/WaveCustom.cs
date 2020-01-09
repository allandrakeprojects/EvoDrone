using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCustom : MonoBehaviour
{
    void OnTransformChildrenChanged()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
