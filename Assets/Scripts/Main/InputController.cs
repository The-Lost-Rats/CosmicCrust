using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : PauseableBehaviour
{
    public static InputController Instance { get; private set; }

    public static Vector3 grabMouseOffset = new Vector3(-.06f, .06f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /***************
     * Cursor Info *
     ***************/

    public enum CursorType
    {
        Default,
        Pointer,
        Grabber,
        GrabberHover,
    }

    [System.Serializable]
    public class CursorInfo
    {
        public CursorType cursorType;
        public Texture2D texture;
        public Vector2 hotspot;
    }

    [SerializeField] private List<CursorInfo> cursors;
    [SerializeField] private List<CursorInfo> webGLCursors;

    private void SetActiveCursor(CursorType cursorType)
    {
#if UNITY_WEBGL
        foreach (CursorInfo cursorInfo in webGLCursors)
#else
        foreach (CursorInfo cursorInfo in cursors)
#endif
        {
            if (cursorInfo.cursorType == cursorType)
            {
#if UNITY_WEBGL
                Cursor.SetCursor(cursorInfo.texture, cursorInfo.hotspot, CursorMode.ForceSoftware);
#else
                Cursor.SetCursor(cursorInfo.texture, cursorInfo.hotspot, CursorMode.Auto);
#endif
            }
        }
    }

    /********************
     * Input Controller *
     ********************/

    public enum InputState
    {
        Default,
        Grabbing
    }

    // Future Justin note: Don't need a list here, will only ever have 1 entry
    private List<InteractableObject> interactableList;
    private InteractableObject currGrabbing;

    public InputState inputState;

    void Start()
    {
        SetActiveCursor(CursorType.Default);
        inputState = InputState.Default;

        interactableList = new List<InteractableObject>();
    }

    public void SetDefaultCursor()
    {
        SetActiveCursor(CursorType.Default);
        inputState = InputState.Default;

        interactableList.Clear();
    }

    public void EnterInteractableObject(InteractableObject interactableObject)
    {
        if (interactableList.Count > 0)
        {
            ExitInteractableObject(interactableList[0]);
        }

        if (inputState == InputState.Default)
        {
            interactableObject.OnEnter();
            if (interactableObject.isInteractable)
            {
                SetActiveCursor(CursorType.Pointer);
            }
        }
        else if (inputState == InputState.Grabbing)
        {
            if (currGrabbing.interactableObjects.Contains(interactableObject.name))
            {
                SetActiveCursor(CursorType.GrabberHover);
            }
        }
        interactableList.Insert(0, interactableObject);
    }

    public void ExitInteractableObject(InteractableObject interactableObject)
    {
        if (!interactableList.Contains(interactableObject))
        {
            Debug.LogError("Error, object already gone: " + interactableObject.name);
            return;
        }

        if (inputState == InputState.Default)
        {
            if (interactableObject.name == interactableList[0].name)
            {
                interactableList[0].OnExit();
                SetActiveCursor(CursorType.Default);
            }
        }
        else if (inputState == InputState.Grabbing)
        {
            bool setCursorGrabHover = false;
            foreach (InteractableObject interactable in interactableList)
            {
                if (interactable.name != interactableObject.name && currGrabbing.interactableObjects.Contains(interactable.name))
                {
                    setCursorGrabHover = true;
                }
            }
            SetActiveCursor(setCursorGrabHover ? CursorType.GrabberHover : CursorType.Grabber);
        }
        interactableList.Remove(interactableObject);
    }

    protected override void LocalUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (interactableList.Count > 0 && interactableList[0].isInteractable)
            {
                inputState = interactableList[0].OnClick();
                SetActiveCursor(interactableList[0].IsGrabbable() ? CursorType.Grabber : CursorType.Default);
                currGrabbing = interactableList[0];
                interactableList[0].OnExit();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (currGrabbing != null)
            {
                inputState = currGrabbing.OnRelease(interactableList);
                currGrabbing = null;

                if (interactableList.Count > 0)
                {
                    interactableList[0].OnEnter();
                    if (interactableList[0].isInteractable)
                    {
                        SetActiveCursor(CursorType.Pointer);
                    }
                    else
                    {
                        SetActiveCursor(CursorType.Default);
                    }
                }
                else
                {
                    SetActiveCursor(CursorType.Default);
                }
            }
        }
    }
}
