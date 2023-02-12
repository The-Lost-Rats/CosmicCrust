using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceHandle : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    public Gradient marinaraGradient;
    public Gradient alfredoGradient;
    public Gradient bbqGradient;

    public ParticleSystem sauceParticles;
    public SauceWheel sauceWheel;

    private float[] movementBounds = new float[]{ -0.2f, 0.185f };
    private float[] saucePositions;
    private float[] localSaucePositions = new float[]{ 1, -10.5f, -22 };

    private const int numSauces = 3;

    private bool isMoving;
    private float mouseOffset;

    private Types.Sauces currSauce = Types.Sauces.Marinara;
    private Types.Sauces[] sauces = new Types.Sauces[]{
        Types.Sauces.Marinara,
        Types.Sauces.Alfredo,
        Types.Sauces.BBQ
    };

    public override void OnEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f);
    }

    public override void OnExit()
    {
        transform.localScale = new Vector3(1, 1);
    }

    private void Start()
    {
        float step = (movementBounds[1] - movementBounds[0]) / numSauces;
        saucePositions = new float[numSauces];
        for (int i = 0; i < numSauces; i++)
        {
            saucePositions[i] = movementBounds[1] - ((step * i) + (step / 2));
        }

        SetSauce();
    }

    public override InputController.InputState OnClick()
    {
        isMoving = true;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseOffset = transform.position.y - mousePosition.y;
        return InputController.InputState.Grabbing;
    }
 
    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        isMoving = false;
        SetSauce();
        return InputController.InputState.Default;
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Add an offset so the anchor point of the sprite can be ignored
            float posY = Mathf.Clamp(mousePosition.y + mouseOffset, movementBounds[0], movementBounds[1]);
            transform.position = new Vector2(transform.position.x, posY);
        }
    }

    private void SetSauce()
    {
        int closestSauce = 0;
        float minSauceDistance = Mathf.Infinity;
        for (int i = 0; i < numSauces; i++)
        {
            float delta = Mathf.Abs(transform.position.y - saucePositions[i]);
            if (delta < minSauceDistance)
            {
                closestSauce = i;
                minSauceDistance = delta;
            }
        }
        transform.localPosition = new Vector2(transform.localPosition.x, localSaucePositions[closestSauce]);
        currSauce = sauces[closestSauce];
        sauceWheel.currSauce = currSauce;

        ParticleSystem.MainModule particleSystemMain = sauceParticles.main;
        switch (currSauce)
        {
            case Types.Sauces.Marinara:
                particleSystemMain.startColor = marinaraGradient;
                break;
            case Types.Sauces.Alfredo:
                particleSystemMain.startColor = alfredoGradient;
                break;
            case Types.Sauces.BBQ:
                particleSystemMain.startColor = bbqGradient;
                break;
        }
    }
}
