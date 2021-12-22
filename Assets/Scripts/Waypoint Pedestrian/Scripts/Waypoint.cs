﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0, 5)]
    public float Width = 1f;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * Width / 2f;
        Vector3 maxBound = transform.position - transform.right * Width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0, 1));
    }
}
