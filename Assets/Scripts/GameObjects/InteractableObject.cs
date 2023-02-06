using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : PauseableBehaviour
{
    public abstract bool isInteractable { get; }
    public abstract List<string> interactableObjects { get; }

    public virtual InputController.InputState OnClick() { return InputController.InputState.Default; }
    public virtual InputController.InputState OnRelease(List<InteractableObject> interactedObjects)  { return InputController.InputState.Default; }

    public virtual void OnEnter() {}
    public virtual void OnExit() {}

    private void OnMouseEnter() {
        // Only while not paused
        if ( !UnsafeGetGameController().IsGamePaused() )
        {
            InputController.Instance.EnterInteractableObject(this);
        }
    }

    private void OnMouseExit() {
        // Only while not paused
        if ( !UnsafeGetGameController().IsGamePaused() )
        {
            InputController.Instance.ExitInteractableObject(this);
        }
    }

    public virtual bool IsGrabbable() { return true; }
}
