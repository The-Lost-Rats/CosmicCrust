using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : InteractableObject
{
    [Range(5, 20)]
    [SerializeField]
    protected float fallingSpeed = 5;

    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>(); }}

    private bool hitGround = true;
    private bool amHolding = false;

    [SerializeField]
    public Sprite active;

    [SerializeField]
    public Sprite inactive;

    private Vector3 finalPos;
    private Vector3 initialPos;

    void Start()
    {
        GetComponent<ParticleSystem>().Stop();
        initialPos = transform.position;
    }

    public override void OnEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f);
    }

    public override void OnExit()
    {
        transform.localScale = new Vector3(1, 1);
    }

    public override InputController.InputState OnClick()
    {
        amHolding = true;
        hitGround = false;
        GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().sprite  = active;

        return InputController.InputState.Grabbing;
    }

    void Update()
    {
        if (amHolding)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }

        if (!amHolding && !hitGround)
        {
            Vector3 pos = transform.position;
            pos.y -= fallingSpeed * Time.deltaTime;
            transform.position = pos;
            
            if ( Mathf.Abs( pos.y - finalPos.y ) < 0.5f )
            {
                hitGround = true;
            }
        }
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        amHolding = false;
        finalPos = transform.position;
        finalPos.y -= 1.0f;

        GetComponent<ParticleSystem>().Stop();
        GetComponent<SpriteRenderer>().sprite  = inactive;

        return InputController.InputState.Default;
    }

    public void Reset()
    {
        transform.position = initialPos;
    }
}
