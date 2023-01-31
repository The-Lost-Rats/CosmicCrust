using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour {
    public static UIController uicInstance = null; // So this instance can be used in other classes

    // Score stuff
    [SerializeField]
    private int minNumDigits = 2;

    [SerializeField]
    private Transform currScoreDigitTransform;

    [SerializeField]
    private Transform currScoreGridTransform;

    [SerializeField]
    private Transform finalScoreDigitTransform;

    [SerializeField]
    private Transform finalScoreGridTransform;

    private List<Transform> currScoreDigitsDisplayed;

    private List<Transform> finalScoreDigitsDisplayed;

    // Hearts
    public GameObject heartGameObject;
    private List<GameObject> hearts;
    
    public Sprite heartFull;
    public Sprite heartEmpty;

    // Digit sprites
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

        currScoreDigitsDisplayed = new List<Transform>();
        finalScoreDigitsDisplayed = new List<Transform>();
    }

    public void SetCurrentScore( int score )
    {
        SetScore(score, currScoreDigitsDisplayed, currScoreDigitTransform, currScoreGridTransform);
    }

    public void SetFinalScore( int score )
    {
        SetScore(score, finalScoreDigitsDisplayed, finalScoreDigitTransform, finalScoreGridTransform);
    }

    private void SetScore( int score, List<Transform> digitsDisplayed, Transform digitTransform, Transform scoreGridTransform )
    {
        // Loop backwards and destroy digits before rendering
        for (int i = digitsDisplayed.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(digitsDisplayed[i].gameObject);
        }
        digitsDisplayed.Clear();

        int numDigits = Utilities.countDigitsInt(score);

        // Default to 2 digits if less than 2
        if (numDigits < minNumDigits)
        {
            numDigits = minNumDigits;
        }

        // Add digit to horizontal ordering
        for (int i = 0; i < numDigits; i++)
        {
            int digit = score % 10;
            score = score / 10;

            Transform num = Instantiate(digitTransform, scoreGridTransform);
            num.gameObject.GetComponent<SpriteRenderer>().sprite = digits[digit];
            num.gameObject.SetActive(true);

            digitsDisplayed.Add(num);
        }
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
