using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public static User instance;
    public InputField inputField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public string GetInputValue()
    {
        if (inputField != null)
        {
            Debug.Log(inputField.text);
            return inputField.text;
        }
        else
        {
            Debug.LogWarning("InputField is not assigned.");
            return null;
        }
    }
}
