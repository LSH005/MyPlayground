using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeypadManager : MonoBehaviour
{
    public TMP_Text PasswordText;

    public SpriteRenderer spriteRenderer;
    public Sprite doorSprite;

    private int[] passcode = new int[5];
    private int passcodeIndex = 0;
    private bool isDoorOpened = false;
    private string password = "";

    private void Start()
    {
        GeneratePasscode();
        PasswordText.text = "####";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("KeyPad");
        }
    }

    void GeneratePasscode()
    {
        for (int i = 0; i < passcode.Length; i++)
        {
            passcode[i] = Random.Range(0, 10);
        }

        for (int i = 0; i < passcode.Length; i++)
        {
            password += passcode[i].ToString();
        }
        //Debug.Log($"비밀번호 : {password}");
    }

    public void ReleasePassword()
    {
        PasswordText.text = password;
    }

    public void OnButtonClick(int buttonNumber)
    {
        if (isDoorOpened) return;

        if (buttonNumber == passcode[passcodeIndex])
        {
            passcodeIndex++;
            if (passcodeIndex > passcode.Length - 1)
            {
                if (spriteRenderer != null && doorSprite != null)
                {
                    spriteRenderer.sprite = doorSprite;
                }
                isDoorOpened = true;
            }
        }
        else
        {
            passcodeIndex = 0;
        }
    }
}
