using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Check this to force render of overlay - for debugging only
    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    private GameObject menuOverlay;

    [SerializeField]
    private SceneController.Level levelType;

    public void Awake() {
        menuOverlay.SetActive(isActive);
        Init();
    }

    void Update()
    {
        bool showImage = isActive || SceneController.GetCurrentLevel() == levelType;
        menuOverlay.SetActive(showImage);

        if (!showImage)
        {
            CleanUp();
        }
    }

    public virtual void CleanUp() {}

    public virtual void Init() {}
}