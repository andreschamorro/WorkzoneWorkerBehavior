using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private WaypointLap _waypointLap = default;
    [SerializeField] private GameObject[] _prefab = null;
    private List<GameObject> _activeVehicle = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 0.0f, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        int k = Random.Range(0, _prefab.Length);
        var instance = _prefab[k].Spawn(transform.position, transform.rotation);
        instance.GetComponent<VehicleWaypoint>().WaypointLap = _waypointLap;
        instance.GetComponent<VehicleWaypoint>().OnEndPath += EndPath;
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
