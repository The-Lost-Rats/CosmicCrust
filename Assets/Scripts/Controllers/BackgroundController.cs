using UnityEngine;
using UnityEngine.UI;

// Code From: https://sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas
[ExecuteInEditMode]
public class BackgroundController : MonoBehaviour
{
    Image backgroundImage;
    RectTransform rectTransform;
    float ratio;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        rectTransform = backgroundImage.rectTransform;
        ratio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rectTransform)
            return;

        //Scale image proportionally to fit the screen dimensions, while preserving aspect ratio
        if(Screen.height * ratio >= Screen.width)
        {
            rectTransform.sizeDelta = new Vector2(Screen.height * ratio, Screen.height);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width / ratio);
        }
    }
}