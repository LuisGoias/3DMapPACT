using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperNextPhraseButtonLogic : MonoBehaviour
{
    [SerializeField] private HelperManager helperManager;

    public void onNextPhraseClick()
    {
        helperManager.ShowPhrase();
    }
}
