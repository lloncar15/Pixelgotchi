using UnityEngine;

public class PhysicsAdjustorTest : MonoBehaviour
{
    //[SerializeField] private Rigidbody myRigidbody;
    private Vector3 startingEulerAngles;

    private void Start()
    {
        //startingEulerAngles = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        //myRigidbody.position = transform.parent.position;

        //transform.position = transform.parent.position;
        //transform.eulerAngles = startingEulerAngles;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Object collided with " + collision.collider.name, collision.collider);
    }
}
