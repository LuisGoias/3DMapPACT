using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class InformationSerialize
{
    public string gameObjectName;
    public string title;
    public string description;
    public string iconPath;
    public string bannerPath;
    //public Sprite icon;
    //public Sprite banner;
    public Material typeOfBuilding;


    public void SetIconPath(string newPath)
    {
        iconPath = newPath;
    }

    public void SetBannerPath(string newPath)
    {
        bannerPath = newPath;
    }


    /*public void SetIcon(Sprite newSprite)
    {
        icon = newSprite;
    }

    public void SetBanner(Sprite newBanner)
    {
        banner = newBanner;
    }
    */
}
