using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : InteractableObject
{
    [Range(5, 20)]
    [SerializeField]
    protected float fallingSpeed = 5;

    public ParticleSystem waterParticleSystem;

    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>(); }}

    private bool amHolding = false;

    [SerializeField]
    public Sprite active;

    [SerializeField]
    public Sprite inactive;

    private Vector3 initPos;

    void Start()
    {
        initPos = transform.position;
    }

    // TODO: create a popup object class or something that has this on enter and exit
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
        waterParticleSystem.Play();
        GetComponent<SpriteRenderer>().sprite  = active;
        SoundController.scInstance.PlaySingle("itemGrab");
        SoundController.scInstance.PlayLoopingSound("wateringCan");

        return InputController.InputState.Grabbing;
    }

    void Update()
    {
        if (amHolding)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        amHolding = false;

        waterParticleSystem.Stop();
        GetComponent<SpriteRenderer>().sprite  = inactive;
        SoundController.scInstance.StopLoopingSound();

        transform.position = initPos;

        return InputController.InputState.Default;
    }

    public void Reset()
    {
        transform.position = initPos;
    }

    public void Show(bool showWateringCan)
    {
        gameObject.SetActive(showWateringCan);
    }
}
