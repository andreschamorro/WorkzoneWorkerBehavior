using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexPoint : MonoBehaviour
{
    private bool _isActive = false;
    public bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_isActive)
        {
            VortexLap vorlab = transform.parent.GetComponent<VortexLap>();
            if (vorlab)
            {
                vorlab.NextPoint();
            }
            _isActive = false;

        }
    }
}
