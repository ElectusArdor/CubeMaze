using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Vector3 directionOfRotation;

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(directionOfRotation * Time.deltaTime);
    }
}
