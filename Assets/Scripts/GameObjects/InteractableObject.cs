using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract bool isInteractable { get; }
    public abstract List<string> interactableObjects { get; }

    public virtual InputController.InputState OnClick() { return InputController.InputState.Default; }
    public virtual InputController.InputState OnRelease(List<InteractableObject> interactedObjects)  { return InputController.InputState.Default; }

    public virtual void OnEnter() {}
    public virtual void OnExit() {}

    private void OnMouseEnter() {
        InputController.Instance.EnterInteractableObject(this);
    }

    private void OnMouseExit() {
        InputController.Instance.ExitInteractableObject(this);
    }
}
