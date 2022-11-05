using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform, cameraControllerTransform, targetPoint, currentPoint;
    [SerializeField] private float cameraSpeed;

    private float maxRange;
    private bool toPlayer, fromPlayer;

    private void Start()
    {
        maxRange = targetPoint.localPosition.magnitude;

        cameraControllerTransform.position = playerTransform.position;
        currentPoint.localPosition = targetPoint.localPosition.normalized * 0.16f;
        SetCameraTransform(currentPoint);
    }

    /// <summary>
    /// Set position and rotation of camera
    /// </summary>
    private void SetCameraTransform(Transform t)
    {
        transform.position = t.position;
        transform.rotation = targetPoint.rotation;
    }

    /// <summary>
    /// Calculate new camera position
    /// </summary>
    /// <param name="direction"> Direction of movement of camera (to or from player) </param>
    private void Move(int direction)
    {
        currentPoint.localPosition = targetPoint.localPosition * (currentPoint.localPosition.magnitude / targetPoint.localPosition.magnitude + direction * Time.deltaTime * cameraSpeed);
    }

    /// <summary>
    /// Check is there a wall near the camera
    /// </summary>
    /// <param name="range"> Radius of OverlapSphere </param>
    /// <returns></returns>
    private bool CheckWalls(float range)
    {
        bool wallIs = false;
        Collider[] colliders = Physics.OverlapSphere(currentPoint.position, range);
        foreach (Collider col in colliders)
        {
            if (col.tag == "Wall")
            {
                wallIs = true;
                break;
            }
        }
        return wallIs;
    }

    private void FixedUpdate()
    {
        toPlayer = CheckWalls(0.07f);
        if (!toPlayer)
            fromPlayer = !CheckWalls(0.14f);
    }

    void LateUpdate()
    {
        cameraControllerTransform.position = playerTransform.position;  //  Move controller to new player position

        //  Calculate new camera position
        if (toPlayer)
            Move(-1);
        else if (fromPlayer & currentPoint.localPosition.magnitude < maxRange)
            Move(1);

        //  Check max/min range
        if (currentPoint.localPosition.magnitude > maxRange)
            currentPoint.localPosition = targetPoint.localPosition;
        else if (currentPoint.localPosition.y < 0.02f)
            currentPoint.localPosition = targetPoint.localPosition.normalized * 0.05f;

        SetCameraTransform(currentPoint);
    }
}
