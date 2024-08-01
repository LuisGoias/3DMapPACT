using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelperManager : MonoBehaviour
{
    [SerializeField] private GameObject helperImage;
    [SerializeField] private GameObject helperPanel;


    [SerializeField] private ScrollRect helperQuestionScrollView;
    [SerializeField] private GameObject questionPanelPrefab;


    [SerializeField] private GameObject questionTitle;
    [SerializeField] private GameObject anwserDescription;
    [SerializeField] private GameObject okBTN;

    private float targetYPosition = 55f;
    private Vector3 initialPosition;

    //public Helper helper;

    public HelperSerialize helper;

    public bool reachedPosition = false;
    private Coroutine hideCoroutine;

    private List<string> phrasesSaid = new List<string>();

    private RectTransform helperQuestionsContentRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = helperImage.transform.position;
        helperQuestionsContentRectTransform = helperQuestionScrollView.content;

        PopulateScrollWithQuestions(helperQuestionsContentRectTransform);
    }


    // Update is called once per frame
    void Update()
    {

        if (helperImage.transform.position.y == targetYPosition && !reachedPosition)
        {
            helperPanel.SetActive(true);
            reachedPosition = true;
        }
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
    }


    private void PopulateScrollWithQuestions(RectTransform rectTransform)
    {
        for(int i = 0; i < helper.questions.Count; i++)
        {
            string currentQuestion = helper.questions[i];
            GameObject newQuestionGO = Instantiate(questionPanelPrefab, rectTransform);
            newQuestionGO.GetComponentInChildren<Button>().onClick
                .AddListener(delegate { onQuestionClick(currentQuestion); });
            newQuestionGO.GetComponentInChildren<Button>()
                .GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion;
        }
    }


    private void onQuestionClick(string question)
    {

        helperQuestionScrollView.gameObject.SetActive(false);

        int questionIndex = helper.questions.IndexOf(question);
        string anwserFound = helper.answers[questionIndex];

        questionTitle.SetActive(true);
        questionTitle.GetComponent<TextMeshProUGUI>().text = question;

        anwserDescription.SetActive(true);
        anwserDescription.GetComponent<TextMeshProUGUI>().text = anwserFound;

        okBTN.SetActive(true);
    }

    public void onExitClick()
    {
        StartCoroutine(HideAfterDelay(0f));
    }

    public bool isHelperTalking()
    {
        if (helperImage.transform.position.y == targetYPosition)
        {
            return true;
        }
        return false;
    }

    public HelperSerialize GetHelperSerialize() { return helper; }
    
    public void SetHelperSerialize(HelperSerialize newHelper) { helper = newHelper; }
}
