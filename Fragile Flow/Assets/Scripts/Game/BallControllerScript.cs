using UnityEngine;

public class BallControllerScript : MonoBehaviour
{
    public float ForwardSpeed = 1.5f;

    Rigidbody ParentRB;
    Rigidbody BallRB;
    Transform BallTransform;

    void Start()
    {
        ParentRB = transform.parent.GetComponent<Rigidbody>();
        BallRB = GetComponent<Rigidbody>();
        BallTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        Vector3 lv = BallRB.linearVelocity;
        lv = new Vector3(lv.x += h, lv.y, lv.z);
        BallRB.linearVelocity = lv;
        ParentRB.linearVelocity = lv;

        float rotationAmount = ForwardSpeed * Mathf.Rad2Deg * Time.fixedDeltaTime;
        BallRB.MoveRotation(BallRB.rotation * Quaternion.Euler(rotationAmount, 0, 0));
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("Ball broke!");
            Time.timeScale = 0f;
        }
    }
}
