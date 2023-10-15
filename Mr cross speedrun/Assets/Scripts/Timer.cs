using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Http;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    public bool ended = false;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ended == false)
        {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            timerText.text = currentTime.ToString("0.000");
        }
    }

    public void BoogerAids()
    {
        ended = true;
    }
}
