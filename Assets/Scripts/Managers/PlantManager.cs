using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager pmInstance = null;

    [SerializeField]
    List<PepperPlant> plants;

    public void Awake() {
        if ( null == pmInstance ) {
            pmInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    public void WipePlants()
    {
        foreach (PepperPlant plant in plants)
        {
            plant.Reset();
        }
    }
}
