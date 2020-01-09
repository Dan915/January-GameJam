using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public DreamType portalType;
    public Material dissolveEffect;

    bool started = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(started) return;
        if(collider.tag != "Player") return;
        if(collider.GetComponent<PlayerController>().dreamType == portalType)
            {if(portalType == DreamType.BAD_DREAM) GameManager.instance.isRedInPortal = true;
            else GameManager.instance.isBlueInPortal = true;}

        if(GameManager.instance.isBlueInPortal && GameManager.instance.isRedInPortal)
        {
            started = true;
            foreach(var item in GameObject.FindGameObjectsWithTag("Player"))
            {
                item.GetComponent<SpriteRenderer>().material = dissolveEffect;
                item.transform.right = Vector2.right;
            }

            StartCoroutine(DissolveCrap());
            Invoke("Go", 1.5f);
        }
    }

    void Go()
        => GameManager.instance.NextLevel();

    IEnumerator DissolveCrap()
    {
        yield return new WaitForSeconds(1.0f);
        foreach(var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            item.GetComponent<SpriteRenderer>().material.SetFloat("_Threshold", item.GetComponent<SpriteRenderer>().material.GetFloat("_Threshold") + 0.1f);
            if(item.GetComponent<SpriteRenderer>().material.GetFloat("_Threshold") >= 1.0f)
            {
                GameManager.instance.NextLevel();
            }
            else
            {
                StartCoroutine(DissolveCrap());
            }
        }

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(started) return;
        if(collider.tag != "Player") return;
        if(collider.GetComponent<PlayerController>().dreamType == DreamType.GOOD_DREAM)
            {GameManager.instance.isBlueInPortal = false;
            Debug.Log("bye blue");}
        else
            GameManager.instance.isRedInPortal = false;
    }
}
