using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Transform container;   //  Empty object on the scene which used to determinate direction of movement
    [SerializeField] private float speed;

    private Rigidbody rb;
    private Quaternion rotationL, rotationR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotationL = Quaternion.Euler(0, -120 * Time.deltaTime, 0);
        rotationR = Quaternion.Euler(0, 120 * Time.deltaTime, 0);
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
            rb.angularVelocity = -container.forward * speed;
        else if (Input.GetKey(KeyCode.S))
            rb.angularVelocity = container.forward * speed;
        else
            rb.angularVelocity = Vector3.zero;
    }

    private void Turning()
    {
        if (Input.GetKey(KeyCode.A))
            Turn(rotationL);
        else if (Input.GetKey(KeyCode.D))
            Turn(rotationR);
    }

    private void Turn(Quaternion q)
    {
        container.rotation *= q;
    }

    void Update()
    {
        Turning();
    }

    private void FixedUpdate()
    {
        Movement();
    }
}
