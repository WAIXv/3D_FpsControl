using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public Vector3 centerPoint;
    public float range;
    public float reActiveTime = 0.1f;

    void Start()
    {
        centerPoint = transform.position;
    }
}
