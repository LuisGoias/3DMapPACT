using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CloseInfoLogic : MonoBehaviour
{
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject informationTitle;
    [SerializeField] private GameObject informationDescription;
    [SerializeField] private GameObject informationBanner;
    [SerializeField] private GameObject informationScroll;

    [SerializeField] private GameObject goBackButton;
    [SerializeField] private GameObject helpButton;
    [SerializeField] private GameObject searchButton;
    [SerializeField] private GameObject currentLocationTMP;
    public void onClickClose()
    {
        informationPanel.gameObject.SetActive(false);
        informationScroll.gameObject.SetActive(false);

        informationTitle.GetComponentInChildren<TextMeshProUGUI>().text = "";
        informationDescription.GetComponentInChildren<TextMeshProUGUI>().text = "";
        informationBanner.GetComponent<Image>().sprite = null;

        goBackButton.SetActive(true);
        helpButton.SetActive(true);
        searchButton.SetActive(true);
        currentLocationTMP.SetActive(true);

    }
}
