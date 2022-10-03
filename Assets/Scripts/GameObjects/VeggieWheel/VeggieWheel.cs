using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieWheel : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    private bool isGrabbed;
    private float angleOffset;
    private float lastAngle;

    // [SerializeField]
    // public float rotationSpeed = 10.0f;

    // [SerializeField]
    // public float maxSpeed = 300.0f;

    // [SerializeField]
    // public float direction = 1;

    // private float angle = 0.0f;

    // public float initialSpeed;

    // Start is called before the first frame update
    // void Start()
    // {
    //     // initialSpeed = rotationSpeed;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     // Rotate the object around its local Z axis at defined speed
    //     // angle += rotationSpeed * direction * Time.deltaTime;
    //     // if (angle > 360)
    //     // {
    //     //     angle = 0;
    //     // }

    //     // transform.rotation = Quaternion.Euler(0, 0, angle);
    // }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override InputController.InputState OnClick()
    {
        isGrabbed = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        angleOffset = Vector2.SignedAngle(Vector2.right, direction) - transform.eulerAngles.z;
        lastAngle = angleOffset;
        return InputController.InputState.Grabbing;
    }
 
    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        isGrabbed = false;
        return InputController.InputState.Default;
    }

    private void Update()
    {
        if (isGrabbed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction) - angleOffset;
            transform.eulerAngles = new Vector3 (0, 0, angle);

            lastAngle = angle;

            

            // var body = GetComponent<Rigidbody2D>();
            // var impulse = (angle * Mathf.Deg2Rad) * body.inertia;

            // body.AddTorque(impulse, ForceMode2D.Impulse);
        }
    }
}
