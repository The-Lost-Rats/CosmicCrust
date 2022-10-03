using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public static WheelController wcInstance = null;

    [SerializeField]
    private VeggieWheel veggieWheel;

    public void Awake() {
        if ( null == wcInstance ) {
            wcInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    public void UpdateSpeedAndDirection()
    {
        // if (veggieWheel.rotationSpeed < veggieWheel.maxSpeed)
        // {
        //     veggieWheel.rotationSpeed = Mathf.Pow(veggieWheel.rotationSpeed, 1.12f);
        // }

        // if (veggieWheel.rotationSpeed > veggieWheel.maxSpeed)
        // {
        //     veggieWheel.rotationSpeed = veggieWheel.maxSpeed;
        // }

        // float flipDirection = Random.Range(0.0f, 1.0f);
        // if (flipDirection > 0.78)
        // {
        //     veggieWheel.direction *= -1;
        // }
    }

    public void Reset()
    {
        // veggieWheel.direction = 1;
        // veggieWheel.rotationSpeed = veggieWheel.initialSpeed;
    }
}
