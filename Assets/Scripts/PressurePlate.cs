using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public List<GateControl> targetGates = new List<GateControl>();

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag != "Player" && collider.tag != "HeavyObject") return;
        foreach(var gate in targetGates.Where(x=>x.isGateOpen == false))
            gate.SetGate(true, gameObject);
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag != "Player" && collider.tag != "HeavyObject" ) return;
        foreach(var gate in targetGates.Where(x=>x.isGateOpen == true))
            gate.SetGate(false, gameObject);
        
    }
}
