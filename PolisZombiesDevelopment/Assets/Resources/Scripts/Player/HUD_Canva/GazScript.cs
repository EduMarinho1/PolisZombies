using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazScript : MonoBehaviour
{
    bool gazActive;

    void Start()
    {
        gazActive = false;
        // Get the Image component and modify the alpha value of its color
        Image image = GetComponent<Image>();
        Color color = image.color;
        color.a = 0f; // Set alpha to 0 (fully transparent)
        image.color = color; // Assign the modified color back to the Image component
    }

    public void setGazActivation(bool activationBoolean)
    {
        gazActive = activationBoolean;

        // Get the Image component and modify the alpha value of its color
        Image image = GetComponent<Image>();
        Color color = image.color;

        if (activationBoolean)
        {
            color.a = 1f; // Set alpha to 0 (fully transparent)
        }
        else
        {
            color.a = 0f; // Set alpha to 1 (fully opaque)
        }

        image.color = color; // Assign the modified color back to the Image component
    }

    public bool getGazActivation()
    {
        return gazActive;
    }
}