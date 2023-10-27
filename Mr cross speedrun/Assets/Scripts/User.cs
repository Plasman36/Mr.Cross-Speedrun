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
    }

    public string GetInputValue()
    {
            return inputField.text;
            DontDestroyOnLoad(gameObject);
    
    }
}
