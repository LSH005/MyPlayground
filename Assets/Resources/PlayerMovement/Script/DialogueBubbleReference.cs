using TMPro;
using UnityEngine;

public class DialogueBubbleReference : MonoBehaviour
{
    public TextMeshPro dialogueText;
    public GameObject bubbleBody;
    public GameObject upperTail;
    public GameObject lowerTail;

    public float bubbleOffsetLimit = 1.0f;


    public void SetTailToLower(bool isLower)
    {
        if (isLower)
        {
            upperTail.SetActive(false);
            lowerTail.SetActive(true);
        }
        else
        {
            upperTail.SetActive(true);
            lowerTail.SetActive(false);
        }
    }

    public void SetBubbleOffset(float offsetDistance)
    {
        if (bubbleOffsetLimit == 0.0f)
        {
            bubbleBody.transform.localPosition = Vector3.zero;
        }
        else
        {
            if (bubbleOffsetLimit < 0)
            {
                bubbleOffsetLimit = Mathf.Abs(bubbleOffsetLimit);
            }

            bubbleBody.transform.localPosition = new Vector3(
                Mathf.Clamp(offsetDistance, -bubbleOffsetLimit, bubbleOffsetLimit),
                0,
                0
            );
        }
    }

    public void SetText(string text)
    {
        dialogueText.text = text;
    }

    public void SetPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
}
