using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using System.Net.Http;
using UnityEngine.Networking;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    public bool ended = false;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Username")]
    string inputValue = User.instance.GetInputValue();

    [Header("Level")]
    public LoadLevel skip = null;

    public class ScoreData
    {
        public string Name;
        public float Score;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(inputValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (ended == false)
        {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            timerText.text = currentTime.ToString("0.00");
        }
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ended = true;

            string apiUrl = "https://leaderboard-n1vc.onrender.com/leaderboard/";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a ScoreData object with the "Name" as a string and "Score" as a float
                    string small = currentTime.ToString("0.00");
                    float smaller = float.Parse(small);
                    ScoreData scoreData = new ScoreData
                    {
                        Name = inputValue,
                        Score = smaller,
                        
                    };

                    // Serialize the ScoreData object to JSON
                    string jsonContent = JsonUtility.ToJson(scoreData);
                    Debug.Log("JSON Content:");
                    Debug.Log(jsonContent);

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send the POST request and await the response
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Debug.Log("Response:");
                        Debug.Log(responseContent);
                    }
                    else
                    {
                        Debug.LogError("Error: " + response.StatusCode);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                }
            }
        }
        skip.LoadNewLevel();
    }
}