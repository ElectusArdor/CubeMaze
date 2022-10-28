using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    public bool isFinished;

    void Start()
    {
        isFinished = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFinished = true;
        }
    }
}
