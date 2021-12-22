using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagger : MonoBehaviour
{
    private List<GameObject> vehiclesInIntersection = new List<GameObject>();
    [SerializeField]
    private bool _flagInRed = true;

    #region PUBLIC

    public string FlagColor()
    {
        if (_flagInRed)
        {
            return "Red";
        }
        return "Green";
    }

    #endregion

    public void SwitchFlagLight()
    {
        _flagInRed = !_flagInRed;
        //Wait few seconds after light transition before making the other car move
        if (!_flagInRed)
        {
            MoveVehiclesQueue();  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_flagInRed)
        {
            TriggerStop(other.gameObject);
        }
    }

    void TriggerStop(GameObject vehicle)
    {
        vehicle.GetComponent<VehicleMove>().ExternalStop = true;
        vehiclesInIntersection.Add(vehicle);
    }

    void OnTriggerExit(Collider other)
    {
        ExitStop(other.gameObject);
    }

    void ExitStop(GameObject vehicle)
    {
        vehicle.GetComponent<VehicleMove>().ExternalStop = false;
        vehiclesInIntersection.Remove(vehicle);
    }

    void MoveVehiclesQueue()
    {
        //Move all vehicles in queue
        List<GameObject> nVehiclesQueue = new List<GameObject>(vehiclesInIntersection);
        foreach (GameObject vehicle in vehiclesInIntersection)
        {
            vehicle.GetComponent<VehicleMove>().ExternalStop = false;
            nVehiclesQueue.Remove(vehicle);
        }
        vehiclesInIntersection = nVehiclesQueue;
    }
}
