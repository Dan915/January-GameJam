using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    public bool isGateOpen = false;
    private GameObject _lastToggler;
    private Vector2 originalPos;
    public Vector2 openPos;
    public float speed = 2;

    bool gateMoving = false;

    void Update()
    {
        if(!gateMoving) return;

        if(isGateOpen)
        {
            OpenGate();
            if((Vector2)transform.position == openPos) gateMoving= false;
        } else
            {
                CloseGate();
                if((Vector2)transform.position == originalPos) gateMoving= false;
            }

    }

    void Start()
    {
        originalPos = transform.position;
    }

    public void ToggleGate(GameObject toggler)
    {
        if(_lastToggler == null) _lastToggler = toggler;
        if(_lastToggler != toggler) return;
        if(isGateOpen)
        {
            CloseGate();
            _lastToggler = null;
        }
        else
        {
            if(gateMoving)
            OpenGate();
        }

        isGateOpen = !isGateOpen;
    }

    public void ForceToggleGate(GameObject toggler)
    {
        _lastToggler = toggler;
        ToggleGate(toggler);
    }

    public void SetForceGate(bool openGate, GameObject toggler)
    {
        _lastToggler = toggler;
        SetGate(openGate, toggler);
    }

    public void SetGate(bool openGate, GameObject toggler)
    {
        if(_lastToggler == null) _lastToggler = toggler;
        if(_lastToggler != toggler) return;
        if(openGate)
        {
            OpenGate();
            isGateOpen = true;
        }
        else
        {
            CloseGate();
            isGateOpen = false;
            _lastToggler = null;
        }
    }

    void OpenGate()
    {
        gateMoving = true;
        float step = speed * Time.deltaTime;
        // transform.position = Vector2.MoveTowards(transform.position, openPos, step);
        transform.position = openPos;
    }

    void CloseGate()
    {
        gateMoving = true;
        float step = speed * Time.deltaTime;
        //transform.position = Vector2.MoveTowards(openPos, originalPos, step);
        transform.position = originalPos;
    }
}
