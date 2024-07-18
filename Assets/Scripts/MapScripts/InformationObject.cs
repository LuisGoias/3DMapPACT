using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Information")]
public class InformationObject : ScriptableObject
{
    public string title;
    public string description;
    public Sprite icon;
    public Sprite banner;
    public Material typeOfBuilding;
}
