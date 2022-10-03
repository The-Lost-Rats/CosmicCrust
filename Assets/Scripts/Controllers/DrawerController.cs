using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [SerializeField]
    private List<CabinetDrawer> drawers;

    public static DrawerController dcInstance = null;

    public void Awake() {
        if ( null == dcInstance ) {
            dcInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    public void Reset()
    {
        foreach (CabinetDrawer drawer in drawers)
        {
            drawer.Reset();
        }
    }
}
