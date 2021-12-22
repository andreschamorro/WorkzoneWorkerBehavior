using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawnerOne : MonoBehaviour
{
    [SerializeField] private WaypointLap _waypointLap = default;
    [SerializeField] private GameObject[] _prefab = null;
    [SerializeField] private Vector3[] _positions = null;
    private List<GameObject> _activeVehicle = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var pos in _positions)
        {
            int k = Random.Range(0, _prefab.Length);
            var instance = _prefab[k].Spawn(pos, transform.rotation);
            instance.GetComponent<VehicleWaypoint>().WaypointLap = _waypointLap;
            instance.GetComponent<VehicleWaypoint>().OnEndPath += EndPath;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndPath(GameObject obj)
    {
        var vehicle = obj.GetComponent<VehicleWaypoint>();
        if (vehicle)
        {
            vehicle.isDrive = true;
            vehicle.PointIndex = -1;
            vehicle.GetNextPoint();
        }
        obj.Despawn();
    }
}
