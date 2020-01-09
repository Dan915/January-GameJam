using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract void Use(GameObject interactingUser);

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag != "Player" || collider.GetComponent<PlayerController>() == null) return;
        var playerController = collider.GetComponent<PlayerController>();

        playerController.CanInteract(true);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag != "Player" || collider.GetComponent<PlayerController>() == null) return;
        var playerController = collider.GetComponent<PlayerController>();

        playerController.CanInteract(false);
    }
}
