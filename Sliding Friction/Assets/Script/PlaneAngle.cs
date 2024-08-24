using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAngle : MonoBehaviour
{
    // Function to get the angle of rotation around the Z-axis
    public float GetZAxisRotationAngle()
    {
        float angle = transform.eulerAngles.z;

        // Normalize the angle to be between 0° and 90°
        if (angle > 180f)
        {
            angle -= 360f;
        }

        // Return the Z-axis rotation angle in degrees
        return angle;
    }

    void Update()
    {

    }
}
