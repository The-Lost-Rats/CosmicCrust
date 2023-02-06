using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PauseableBehaviour : MonoBehaviour
{
    protected GameController gcInstance;
    protected virtual void LocalUpdate() { }

    protected GameController UnsafeGetGameController()
    {
        if (gcInstance == null)
        {
            gcInstance = GameController.gcInstance;
        }

        return (gcInstance);
    }

    void Update()
    {
        if ( gcInstance == null )
        {
            UnsafeGetGameController();
        }

        if ( gcInstance != null && !gcInstance.IsGamePaused() )
        {
            LocalUpdate();
        }
    }
}
