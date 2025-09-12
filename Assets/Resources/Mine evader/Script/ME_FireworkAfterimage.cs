using UnityEngine;
using System.Collections;

public class ME_FireworkAfterimage : MonoBehaviour
{
    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public void Setting(Color32 myColor, Vector3 scale)
    {
        transform.localScale = scale;
        spr.color = myColor;

        StartCoroutine(ShrinkOverTime());
    }

    private IEnumerator ShrinkOverTime()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float elapsedTime = 0;
        float ActionTime = Mathf.Min(transform.localScale.x, transform.localScale.y);

        while (elapsedTime < ActionTime)
        {
            float t = elapsedTime / ActionTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
