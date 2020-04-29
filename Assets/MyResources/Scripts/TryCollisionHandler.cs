using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with Try Zone: " + other.name);
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().IncreaseTries();
        }
    }
}
