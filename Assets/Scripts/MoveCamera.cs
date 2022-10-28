using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void LateUpdate()
    {
        transform.position = playerTransform.position;
    }
}
