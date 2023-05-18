using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObject : GrabbableObject
{
    protected override void OnUpdate(Vector2 mousePos)
    {
        transform.position = mousePos;
    }

    public void Drop(bool onPizza)
    {
        if (onPizza)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            DropObject();
        }
    }

    public void SetDroppableSprite(Texture2D texture)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
