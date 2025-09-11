using UnityEngine;
using System.Collections;

public class ME_FlagAction : MonoBehaviour
{
    public float WaitTime = 0.5f;
    public float ActionTime = 1f;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(3f, 6f));
        rb2d.angularVelocity = Random.Range(-180f, 180f);
        StartCoroutine(ShrinkOverTime());
    }

    private IEnumerator ShrinkOverTime()
    {
        yield return new WaitForSeconds(WaitTime);

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float elapsedTime = 0f;

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
