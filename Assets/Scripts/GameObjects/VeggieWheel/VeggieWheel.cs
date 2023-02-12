using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieWheel : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    public List<VeggieQuad> veggieQuads;

    private bool isGrabbed;
    private float angleOffset;
    private float lastAngleDiff;

    private int turnDirection = 1;

    [SerializeField]
    public float rotationSpeed = 10.0f;

    public Vector3 initialWheelAngle;

    public List<Vector3> initialQuadAngles;

    public override void OnEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override InputController.InputState OnClick()
    {
        isGrabbed = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        angleOffset = Vector2.SignedAngle(Vector2.right, direction) - transform.eulerAngles.z;
        return InputController.InputState.Grabbing;
    }
 
    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        isGrabbed = false;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction) - angleOffset;
        turnDirection = lastAngleDiff >= 0 ? 1 : -1;
        return InputController.InputState.Default;
    }

    private void Start()
    {
        initialWheelAngle = transform.eulerAngles;
        initialQuadAngles = new List<Vector3>();
        for (int i = 0; i < veggieQuads.Count; i++)
        {
            initialQuadAngles.Add(veggieQuads[i].transform.eulerAngles);
        }
    }

    private void Update()
    {
        float angle;
        if (isGrabbed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            angle = Vector2.SignedAngle(Vector2.right, direction) - angleOffset;
            lastAngleDiff = angle - transform.eulerAngles.z;
        }
        else
        {
            angle = transform.eulerAngles.z + (rotationSpeed * turnDirection * Time.deltaTime);
        }
        transform.eulerAngles = new Vector3 (0, 0, angle);
        for (int i = 0; i < veggieQuads.Count; i++)
        {
            veggieQuads[i].transform.eulerAngles = new Vector3(0, 0, angle + (90 * i));
        }
    }

    public void Reset()
    {
        transform.eulerAngles = initialWheelAngle;
        for (int i = 0; i < veggieQuads.Count; i++)
        {
            veggieQuads[i].transform.eulerAngles = initialQuadAngles[i];
        }
    }
}
