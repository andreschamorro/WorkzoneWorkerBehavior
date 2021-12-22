using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FlaggerSystem : MonoBehaviour
{

    #region FIELDS

    [Serializable]
    public class FlaggerList
    {
        [SerializeField]
        public FlaggerSystem _flagsys = default;

        [SerializeField]
        public GameObject[] _flaggers = new GameObject[0];
    }

    [SerializeField]
    private float _proceedTime = 20.0f;
    [SerializeField]
    private float _standTime = 20.0f;

    public FlaggerList _flaggerList = new FlaggerList();
    private int _numFlaggers;
    // private bool _onStand;
    private int _onProceedFlag;

    [SerializeField]
    private bool _isAlwaysDraw = false;

    #endregion

    #region PROPERTIES
    private GameObject[] FlaggersData => _flaggerList._flaggers;

    #endregion

    #region UNITY_METHODS
    // Start is called before the first frame update
    void Start()
    {
        _onProceedFlag = 0;
        // _onStand = false;
        _numFlaggers = FlaggersData.Length;
        InvokeRepeating("ControllerState", 0.0f, _standTime + _proceedTime);
    }

    void ControllerState()
    {
        StartCoroutine(ProceedState());
    }

    IEnumerator ProceedState()
    {
        FlaggersData[_onProceedFlag].GetComponent<Flagger>().SwitchFlagLight();
        yield return new WaitForSeconds(_proceedTime);
        FlaggersData[_onProceedFlag].GetComponent<Flagger>().SwitchFlagLight();
        _onProceedFlag = (_onProceedFlag + 1) % _numFlaggers;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawGizmos(_isAlwaysDraw);

        foreach (var flag in FlaggersData)
        {
            if (Selection.Contains(flag.gameObject))
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

    #endregion

    #region PUBLIC_METHODS
    
    #endregion

    #region PRIVATE_METHODS
    private void DrawGizmos(bool selected)
    {
        if (!selected)
        {
            return;
        }

        _flaggerList._flagsys = this;

        foreach (var child in FlaggersData)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(child.transform.position, 0.5f);
        }
    }
    private void CreateData()
    {
        if (FlaggersData.Length > 1)
        {
            _numFlaggers = FlaggersData.Length;
        }
    }
    #endregion
}
