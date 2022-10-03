using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCheese : GrabbableObject
{
    public Gradient ballCheeseGradient;
    public Gradient cubeCheeseGradient;
    public Gradient triangleCheeseGradient;

    private ParticleSystem cheeseGraterParticles;
    private Bounds cheeseBounds;

    private Constants.CheeseTypes cheeseType;
    private Vector2 lastPos;
    private bool overGrater = false;

    [SerializeField]
    [Range(0.1f, 0.5f)]
    private const float grateTimerStart = 0.2f;
    private float grateTimer = 0;

    [SerializeField]
    [Range(1, 5)]
    private float cheeseAddedTime = 1;
    private float cheeseTimer = 0;
    private bool cheeseAdded = false;

    protected override void OnUpdate(Vector2 mousePos)
    {
        transform.position = mousePos;

        CheckForGrate();
        if (grateTimer > 0)
        {
            grateTimer -= Time.deltaTime;
            if (grateTimer <= 0)
            {
                cheeseGraterParticles.Stop();
            }
            else if (cheeseBounds.pizzaInBounds)
            {
                cheeseTimer += Time.deltaTime;
                if (!cheeseAdded && cheeseTimer >= cheeseAddedTime)
                {
                    // TODO Force release mouse
                    PlayController.instance.SetCheese(cheeseType);
                    cheeseAdded = true;
                }
            }
        }

        lastPos = transform.position;
    }

    private void CheckForGrate()
    {
        if (overGrater && Mathf.Abs(transform.position.y - lastPos.y) > 0)
        {
            if (!cheeseGraterParticles.isPlaying)
            {
                cheeseGraterParticles.Play();
            }
            grateTimer = grateTimerStart;
        }
    }

    public void CopyCheeseValues(TableCheese parentCheese)
    {
        cheeseType = parentCheese.cheeseType;

        ParticleSystem[] particleSystems = GameObject.FindObjectsOfType<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem.name == "CheeseParticleSystem")
            {
                cheeseGraterParticles = particleSystem;
            }
        }
        ParticleSystem.MainModule particleSystemMain = cheeseGraterParticles.main;
        switch (cheeseType)
        {
            case Constants.CheeseTypes.Ball:
                particleSystemMain.startColor = ballCheeseGradient;
                break;
            case Constants.CheeseTypes.Cube:
                particleSystemMain.startColor = cubeCheeseGradient;
                break;
            case Constants.CheeseTypes.Triangle:
                particleSystemMain.startColor = triangleCheeseGradient;
                break;
        }

        Bounds[] boundsArr = GameObject.FindObjectsOfType<Bounds>();
        foreach (Bounds bounds in boundsArr)
        {
            if (bounds.name == "CheeseBounds")
            {
                cheeseBounds = bounds;
            }
        }

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = parentCheese.GetComponent<SpriteRenderer>().sprite;

        // Code from https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
        Collider2D copyCollider = parentCheese.GetComponent<Collider2D>();
        System.Type type = copyCollider.GetType();
        Component copy = gameObject.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields(); 
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(copyCollider));
        }

        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Grater")
        {
            overGrater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Grater")
        {
            overGrater = false;
        }
    }

    private void OnDestroy()
    {
        cheeseGraterParticles.Stop();
    }
}
