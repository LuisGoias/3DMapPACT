using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InformationSerialize
{
    public string gameObjectName;
    public string title;
    public string description;
    public Sprite icon;
    public Sprite banner;
    public Material typeOfBuilding;


    public void SetIcon(Sprite newSprite)
    {
        icon = newSprite;
    }

    public void SetBanner(Sprite newBanner)
    {
        banner = newBanner;
    }

}
