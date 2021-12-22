using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VortexLap : MonoBehaviour
{
    #region FIELDS

    [Serializable]
    public class VortexList
    {
        [SerializeField]
        public VortexLap _lap = default;

        [SerializeField]
        public Transform[] _items = new Transform[0];
    }
    
    [SerializeField]
    private bool _lap = true;
    [SerializeField]
    private GameObject _vortex = default;
    [SerializeField]
    private bool _isAlwaysDraw = false;
    private int _numPoints;
    private int _currentPoint;

    public VortexList _vortexList = new VortexList();
    public List<Vector3> _curvePoints = new List<Vector3>();
    #endregion

    #region PROPERTIES
    private Transform[] VortexData => _vortexList._items;

    public bool Lap
    {
        get => _lap;
        set => _lap = value;
    }
    #endregion

    #region PUBLIC_METHODS
    public void NextPoint()
    {
        if (VortexData.Length > 1)
        {
            _vortex.transform.position = VortexData[_currentPoint].position;
            var vor = VortexData[_currentPoint].GetComponent<VortexPoint>();
            if (vor)
            {
                vor.isActive = true;
            }
            _currentPoint = (_currentPoint + 1) % _numPoints;
        }
    }
    #endregion

    private void Awake()
    {
        _numPoints = VortexData.Length;
        _currentPoint = 0;
    }

    void Start()
    {
        NextPoint();
    }

# if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawGizmos(_isAlwaysDraw);

        foreach (var vor in VortexData)
        {
            if (Selection.Contains(vor.gameObject))
            {
                DrawGizmos(true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }
#endif

    #region PRIVATE_METHODS
    private void DrawGizmos(bool selected)
    {
        if (!selected)
        {
            return;
        }

        _vortexList._lap = this;

        foreach (var child in VortexData)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(child.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        }

        if (VortexData.Length > 1)
        {
            _numPoints = VortexData.Length;
        }
    }
    #endregion
}
