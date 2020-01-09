using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    public List<GateControl> targetGates = new List<GateControl>();

    public override void Use(GameObject interactingUser)
    {
        foreach(var gate in targetGates)
            gate.ToggleGate(gameObject);
    }
}
