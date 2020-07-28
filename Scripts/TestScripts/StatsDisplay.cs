using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{

    private Text StatTextBox;
    private string StatString = "";

    private void Start()
    {
        StatTextBox = this.GetComponent<Text>();
    }
    void Update()
    {
        StatString = "";

        StatString += "Fps: " +Mathf.RoundToInt( 1 / Time.deltaTime) +"\n";

        StatString += "Poly Count = " + "Above 2 mil  \n" +
                    "Tessallation not included in poly count**" + "\n";

        StatString += "No Realtime Lights**";

        StatTextBox.text = StatString;
    }
}
