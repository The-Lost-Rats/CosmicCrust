using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceWheel : InteractableObject
{
    public ParticleSystem sauceParticles;
    public Bounds bounds;

    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    private bool isGrabbed;
    private float angleOffset;

    private float lastAngle;
    private const float angleDiff = 5;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    private const float turnTimerStart = 0.2f;
    private float turnTimer = 0;

    [SerializeField]
    [Range(1, 5)]
    private float sauceAddedTime = 1;
    private float sauceTimer = 0;
    private bool sauceAdded = false;

    public Constants.Sauces currSauce = Constants.Sauces.Marinara; // Set by SauceHandle

    public override void OnEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f);
    }

    public override void OnExit()
    {
        transform.localScale = Vector3.one;
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
        sauceParticles.Stop();
        turnTimer = 0;
        sauceTimer = 0;
        sauceAdded = false;
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

            CheckForSpray();
            if (turnTimer > 0)
            {
                turnTimer -= Time.deltaTime;
                if (turnTimer <= 0)
                {
                    sauceParticles.Stop();
                }
                else if (bounds.pizzaInBounds)
                {
                    sauceTimer += Time.deltaTime;
                    if (!sauceAdded && sauceTimer >= sauceAddedTime)
                    {
                        PlayController.instance.SetSauce(currSauce);
                        sauceAdded = true;
                    }
                }
            }

            lastAngle = angle;
        }
    }

    private void CheckForSpray()
    {
        if (Mathf.Abs(lastAngle - transform.eulerAngles.z) > angleDiff)
        {
            if (!sauceParticles.isPlaying)
            {
                sauceParticles.Play();
            }
            turnTimer = turnTimerStart;
        }
    }
}
