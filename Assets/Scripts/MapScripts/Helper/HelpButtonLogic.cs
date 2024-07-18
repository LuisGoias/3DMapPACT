using System.Collections;
using UnityEngine;

public class HelpButtonLogic : MonoBehaviour
{
    [SerializeField] private GameObject helper;
    
    private float targetYPosition = -440f;  // Set this to the desired target Y position
    private float speed = 150f;  // Speed of the image movement

    private RectTransform helperRectTransform;
    private bool isMoving = false;  // To check if the coroutine is already running

    void Start()
    {
        // Get the RectTransform component of the helper object
        helperRectTransform = helper.GetComponent<RectTransform>();
    }

    public void onHelpClick()
    {
        // Start the coroutine to move the image if it's not already moving
        if (!isMoving)
        {
            StartCoroutine(MoveHelperUp());
        }
    }

    private IEnumerator MoveHelperUp()
    {
        isMoving = true;

        // Move the image up until it reaches the target Y position
        while (helperRectTransform.anchoredPosition.y < targetYPosition)
        {
            helperRectTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
            yield return null;  // Wait for the next frame
        }

        // Ensure the final position is exactly the target position
        helperRectTransform.anchoredPosition = new Vector2(helperRectTransform.anchoredPosition.x, targetYPosition);
        isMoving = false;
    }
}
