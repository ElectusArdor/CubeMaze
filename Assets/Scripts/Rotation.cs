using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, Time.deltaTime, 0);
    }
}
