using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FetchUserNameScript : MonoBehaviour
{
    public TextMeshProUGUI userNameText; // Drag your TextMeshPro Text object here in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        // Fetch the username of the current computer
        string userName = System.Environment.UserName;
        // Display the username in the TextMeshPro Text
        // Check if userName is null
        if (userName != null)
        {
            // Display the username in the TextMeshPro Text
            userNameText.text = userName;
        }
        else
        {
            // Display default text if userName is null
            userNameText.text = "Player";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
