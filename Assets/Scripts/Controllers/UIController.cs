using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour {
    public static UIController uicInstance = null; // So this instance can be used in other classes

    public GameObject scoreTens;
    public GameObject scoreOnes;

    public GameObject heartGameObject;
    private List<GameObject> hearts;
    
    public Sprite heartFull;
    public Sprite heartEmpty;

    public List<Sprite> digits;

    void Awake()
    {
        if (uicInstance == null) {
            uicInstance = this;
        }
        else if (uicInstance != this) {
            Destroy(gameObject);
        }
    
        hearts = new List<GameObject>();
        foreach ( Transform child in heartGameObject.transform ) {
            hearts.Add( child.gameObject );
        }
    }

    public void SetScore( int score )
    {
        int tens = Mathf.FloorToInt(score / 10);
        int ones = Mathf.FloorToInt(score % 10);

        scoreTens.GetComponent<SpriteRenderer>().sprite = digits[tens];
        scoreOnes.GetComponent<SpriteRenderer>().sprite = digits[ones];
    }

    public void SetHearts( bool lifeLost ) {
        foreach ( GameObject heart in hearts ) {
            SpriteRenderer heartImage = heart.GetComponent<SpriteRenderer>();
            if ( lifeLost ) {
                if ( heartImage.sprite == heartFull ) {
                    heartImage.sprite = heartEmpty;
                    break;
                }
            } else {
                if ( heartImage.sprite == heartEmpty ) {
                    heartImage.sprite = heartFull;
                    break;
                }
            }
        }
    }
}
