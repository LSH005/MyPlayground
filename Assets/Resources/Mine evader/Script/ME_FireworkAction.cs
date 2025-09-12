using UnityEngine;
using System.Collections;

public class ME_FireworkAction : MonoBehaviour
{
    public GameObject afterimage;

    private Rigidbody2D rb2d;
    private SpriteRenderer spr;
    private Color32 myColor;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        myColor = new Color32((byte)Random.Range(150, 256), (byte)Random.Range(150, 256), (byte)Random.Range(150, 256), 255);
    }

    void Start()
    {
        spr.color = myColor;
        rb2d.velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(5f, 10f));
        rb2d.angularVelocity = Random.Range(-360f, 360f);
        transform.localScale = new Vector3(Random.Range(0.1f, 0.5f), Random.Range(0.1f, 0.5f), 1);

        StartCoroutine(ShrinkOverTime());
    }

    private IEnumerator ShrinkOverTime()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float elapsedTime = 0f;
        float ActionTime = Random.Range(1.5f, 2.5f);

        while (elapsedTime < ActionTime)
        {
            float t = elapsedTime / ActionTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            GameObject newAfterimage = Instantiate(afterimage, transform.position, transform.rotation);
            ME_FireworkAfterimage afterImageScript = newAfterimage.GetComponent<ME_FireworkAfterimage>();

            if (afterImageScript != null)
            {
                afterImageScript.Setting(myColor, transform.localScale);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
