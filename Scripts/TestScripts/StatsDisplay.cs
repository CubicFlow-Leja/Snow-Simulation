using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    public GameObject PPCarrier;
    public UnityEngine.PostProcessing.PostProcessingBehaviour postProcessingBehaviour;

    private Text StatTextBox;
    private string StatString = "";


    private float DeltaTime=0.0f;

    public bool TogglePP;
    public bool ToggleOutline;
    public bool TogglePixels;
    public bool ToggleVSync;

    private void Start()
    {
        TogglePP = true;
        ToggleOutline = true;
        TogglePixels = true;
        ToggleVSync = false;


        Application.targetFrameRate = -1;
        StatTextBox = this.GetComponent<Text>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            Cursor.visible = true;
        else
            Cursor.visible = false;


        StatString = "";
        //DeltaTime += (Time.unscaledDeltaTime - DeltaTime) * 0.1f;
        //float msec = DeltaTime * 1000.0f;
        //float fps = 1.0f / DeltaTime;

        //StatString += "Fps: " + fps + "\n";

        StatString += "Poly Count => Above 2 mil  \n";

        StatString += "VSync -->" + ToggleVSync + "\n\n";


        StatString += "Temporal AA -->" + TogglePP + "\n";
        StatString += "Motion Blur -->" + TogglePP + "\n";
        StatString += "Bloom -->" + TogglePP + "\n";
        StatString += "Color Grading Post Process -->" + TogglePP + "\n\n";


        StatString += "Outline Post Process -->" + ToggleOutline + "\n";
        StatString += "Pixelization Post Process -->" + (TogglePixels && ToggleOutline).ToString() + "\n\n";

        StatString += "Hold LCTRL to enable Cursor\n\n";

        StatTextBox.text = StatString;
    }


    public void TogglePixel()
    {
        if (TogglePixels)
        {
            PPCarrier.GetComponent<OutlineController>().UsePixel = false;
            TogglePixels = false;
        }
        else
        {
            PPCarrier.GetComponent<OutlineController>().UsePixel = true;
            TogglePixels = true;
        }
    }


    public void VSync()
    {
        if (ToggleVSync)
        {
            Application.targetFrameRate = -1;
            ToggleVSync = false;
        }
        else
        {
            Application.targetFrameRate = 60;
            ToggleVSync = true;
        }
    }


    public void ToglePostProcess()
    {
        if (TogglePP)
        {
            postProcessingBehaviour.enabled = false;
            TogglePP = false;
        }
        else
        {
            postProcessingBehaviour.enabled = true;
            TogglePP = true;
        }
    }


    public void ToggleOutlines()
    {
        if (ToggleOutline)
        {
            PPCarrier.GetComponent<OutlineController>().enabled = false;
            ToggleOutline = false;
        }
        else
        {
            PPCarrier.GetComponent<OutlineController>().enabled = true;
            ToggleOutline = true;
        }
    }
}
