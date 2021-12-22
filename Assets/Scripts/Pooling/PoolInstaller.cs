﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9999)]
public class PoolInstaller : MonoBehaviour
{
    [SerializeField] private PoolContainer[] _pools = null;

    private void Awake()
    {
        for (int i = 0; i < _pools.Length; i++)
            _pools[i].Populate();
    }

    [System.Serializable]
    private struct PoolContainer
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField, Min(1)] private int _startCount;

        public void Populate() =>
            new Pool(_prefab, _startCount);
    }
}
