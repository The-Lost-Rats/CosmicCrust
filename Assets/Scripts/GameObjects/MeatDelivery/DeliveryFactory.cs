using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Given enum, instantiate correct prefab at locaion and return instance
public class DeliveryFactory : MonoBehaviour
{
    public static DeliveryFactory dfInstance = null;

    [SerializeField]
    public GameObject pepperoniBoxPrefab;

    [SerializeField]
    public GameObject sausageBoxPrefab;

    [SerializeField]
    public GameObject beefBoxPrefab;

    [SerializeField]
    public GameObject chickenBoxPrefab;

    public void Awake() {
        if ( null == dfInstance ) {
            dfInstance = this;
        } else {
            Destroy( this.gameObject );
        }
    }

    public GameObject CreateMeatBox(Constants.Meats meatType)
    {
        GameObject meatBoxPrefab = null;
        GameObject meatBox = null;

        switch(meatType)
        {
            case Constants.Meats.Pepperoni:
                meatBoxPrefab = pepperoniBoxPrefab;
                break;
            case Constants.Meats.Sausage:
                meatBoxPrefab = sausageBoxPrefab;
                break;
            case Constants.Meats.Beef:
                meatBoxPrefab = beefBoxPrefab;
                break;
            case Constants.Meats.Chicken:
                meatBoxPrefab = chickenBoxPrefab;
                break;
        }

        if (meatBoxPrefab != null)
        {
            meatBox = Instantiate(meatBoxPrefab, meatBoxPrefab.transform.position, Quaternion.identity);
            meatBox.SetActive(true);
        }

        return meatBox;
    }
}
