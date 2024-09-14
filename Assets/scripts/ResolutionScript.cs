using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionScript : MonoBehaviour
{
    // Code based on https://www.youtube.com/watch?v=HnvPNoU9Wjw&t=264s

    [SerializeField] private TMP_Dropdown dropDown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        dropDown.ClearOptions(); // Clear dropdown
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]); // Possible resultions with current refresh rate
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            // Add all resolutions to dropdown
            string option = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + "Hz";
            options.Add(option);
            
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i; // Index reference for dropdown
            }
        }

        dropDown.AddOptions(options);
        dropDown.value = currentResolutionIndex;
        dropDown.RefreshShownValue(); // Refresh dropdown display
    }

    public void SetResolution(int index)
    {
        Resolution res = filteredResolutions[index];
        Screen.SetResolution(res.width, res.height, true);
    }

    public void ToggleFullScreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
    }
}
