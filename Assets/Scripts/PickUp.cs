using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        var otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("PickUp"))
        {
            PickUpObject(otherGameObject);
        }
    }

    void PickUpObject(GameObject pickUp)
    {
        // pickUp
    }
}
