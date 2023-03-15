using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCell : ISceneController
{
    override protected GameState GetGameState() { return GameState.PLAY; }

    public SpriteRenderer sr;
    public GameObject checkmark;
    public GameObject overlay;
    public string cellName;

    [System.Serializable]
    public struct HelpObject
    {
        public List<string> toppingNames;
        public List<GameObject> gameObjects;
    }
    public List<HelpObject> helpObjects;

    public void SetTexture(Texture2D texture)
    {
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        sr.sprite.name = texture.name;
        cellName = texture.name;
    }

    public void SetCheckmarkActive(bool active)
    {
        checkmark.SetActive(active);
    }

    private void SetSortingLayer(GameObject go, string sortingLayerName)
    {
        foreach (Transform child in go.transform)
        {
            SetSortingLayer(child.gameObject, sortingLayerName);
        }
        SpriteRenderer goSR = go.GetComponent<SpriteRenderer>();
        if (goSR != null)
        {
            if (go.name == "WateringCan" && sortingLayerName != "Help")
            {
                goSR.sortingLayerName = "GrabbableObjects";
            }
            else
            {
                goSR.sortingLayerName = sortingLayerName;
            }
        }
    }

    private void OnMouseEnter()
    {
       // Abort if we are not in play
        if (!SceneActive())
        {
            return;
        }

        if (InputController.Instance.inputState == InputController.InputState.Default && sr.sprite != null)
        {
            overlay.SetActive(true);
            sr.sortingLayerName = "Help";
            foreach (HelpObject helpObject in helpObjects)
            {
                if (helpObject.toppingNames.Contains(cellName))
                {
                    foreach (GameObject go in helpObject.gameObjects)
                    {
                        SetSortingLayer(go, "Help");
                    }
                }
            }
        }
    }

    private void OnMouseExit()
    {
        // Abort if we are not in play
        if (!SceneActive())
        {
            return;
        }

        sr.sortingLayerName = "GameObjects";
        if (overlay.activeInHierarchy)
        {
            overlay.SetActive(false);
            foreach (HelpObject helpObject in helpObjects)
            {
                if (helpObject.toppingNames.Contains(cellName))
                {
                    foreach (GameObject go in helpObject.gameObjects)
                    {
                        SetSortingLayer(go, "GameObjects");
                    }
                }
            }
        }
    }
}
