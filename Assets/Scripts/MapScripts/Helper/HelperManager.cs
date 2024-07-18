using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    [SerializeField] private GameObject helperImage;
    [SerializeField] private GameObject helperPanel;
    [SerializeField] private GameObject helperText;

    private float targetYPosition = 55f;
    private Vector3 initialPosition;
    public Helper helper;

    public bool reachedPosition = false;
    private Coroutine hideCoroutine;

    private List<string> phrasesSaid = new List<string>();



    // Start is called before the first frame update
    void Start()
    {
        initialPosition = helperImage.transform.position;

    }


    // Update is called once per frame
    void Update()
    {

        if (helperImage.transform.position.y == targetYPosition && !reachedPosition)
        {
            helperPanel.SetActive(true);
            reachedPosition = true;
            ShowPhrase();
        }
    }

    public void ShowPhrase()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        if (!helper.phrases.Any())
        {
            helper.setPhrases(phrasesSaid);
            phrasesSaid.Clear();
        }


        int phraseChosen = Random.Range(0, helper.phrases.Count);
        string chosenPhrase = helper.phrases[phraseChosen];

        helperText.GetComponent<TextMeshProUGUI>().text = chosenPhrase;
        phrasesSaid.Add(chosenPhrase);
        helper.phrases.Remove(chosenPhrase);

        hideCoroutine = StartCoroutine(HideAfterDelay(5f)); // Adjust the delay as needed
    }


    public void HideWhenObjClicked()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterDelay(0f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        helperPanel.SetActive(false);
        helperImage.transform.position = initialPosition;
        reachedPosition = false; // Reset reachedPosition if you want to allow re-triggering
        helper.joinPhrases(phrasesSaid);
    }

    public bool isHelperTalking()
    {
        if (helperImage.transform.position.y == targetYPosition)
        {
            return true;
        }
        return false;
    }
}
