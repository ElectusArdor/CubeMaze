using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsation : MonoBehaviour
{
    [SerializeField] private float pulsationSpeed;

    private Vector3 nativeTransform;
    private float state;

    void Start()
    {
        nativeTransform = transform.localScale / 5f;
    }

    void Update()
    {
        transform.localScale = nativeTransform * (5 + Mathf.Sin(state));
        state += Time.deltaTime * pulsationSpeed;
    }
}
