using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : ISceneController
{
    override protected GameState GetGameState() { return GameState.PLAY; }

    public abstract bool isInteractable { get; }
    public abstract List<string> interactableObjects { get; }

    public virtual InputController.InputState OnClick() { return InputController.InputState.Default; }
    public virtual InputController.InputState OnRelease(List<InteractableObject> interactedObjects)  { return InputController.InputState.Default; }

    public virtual void OnEnter() {}
    public virtual void OnExit() {}

    private void OnMouseEnter() {
        // Abort if we are not in play
        if (!SceneActive())
        {
            return;
        }

        InputController.Instance.EnterInteractableObject(this);
    }

    private void OnMouseExit() {
        // Abort if we are not in play
        if (!SceneActive())
        {
            return;
        }

        InputController.Instance.ExitInteractableObject(this);
    }

    public virtual bool IsGrabbable() { return true; }
}
