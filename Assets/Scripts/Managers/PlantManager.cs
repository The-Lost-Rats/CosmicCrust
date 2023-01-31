using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager pmInstance = null;

    [SerializeField]
    List<PepperPlant> plants;

    [SerializeField]
    private WateringCan wateringCan;

    public void Awake() {
        if ( null == pmInstance ) {
            pmInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    // Reset plants
    public void WipePlants()
    {
        foreach (PepperPlant plant in plants)
        {
            plant.Reset();
        }
    }

    // Reset watering can
    public void ResetWateringCan()
    {
        wateringCan.Reset();
    }

    // Reset everything
    public void Reset()
    {
        WipePlants();
        ResetWateringCan();
    }

    public void ShowWateringCan(bool showWateringCan)
    {
        wateringCan.Show(showWateringCan);
    }
}
