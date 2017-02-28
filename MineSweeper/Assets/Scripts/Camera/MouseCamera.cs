using UnityEngine;
using System.Collections;

[AddComponentMenu("Infinite Camera-Control/Mouse Orbit with zoom")]
public class MouseCamera : MonoBehaviour
{

    public Transform target;
    public Vector3 targetPosition = Vector3.zero;

    public float xSpeed = 12.0f;
    public float ySpeed = 12.0f;
    public float scrollSpeed = 10.0f;

    public float zoomMin = 1.0f;
    public float zoomMax = 20.0f;

    public float distance;
    public Vector3 position;

    public bool isActivated;
    

    float x = 0.0f;
    float y = 0.0f;
    

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }



    void LateUpdate()
    {
        // only update if the mousebutton is held down
        if (Input.GetMouseButtonDown(1))
        {
            isActivated = true;
        }

        // if mouse button is let UP then stop rotating camera
        if (Input.GetMouseButtonUp(1))
        {
            isActivated = false;
        }


        if (isActivated)
        {
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);

            //  get the distance the mouse moved in the respective direction
            /*x += Input.GetAxis("Mouse X") * xSpeed;
            y -= Input.GetAxis("Mouse Y") * ySpeed;
            
            // when mouse moves left and right we actually rotate around local y axis	
            transform.RotateAround(targetPosition, transform.up, x);

            // when mouse moves up and down we actually rotate around the local x axis	
            transform.RotateAround(targetPosition, transform.right, y);*/

            
            // reset back to 0 so it doesn't continue to rotate while holding the button
            x = 0;
            y = 0;
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - scroll, 2);
            }
        }
    }

    
    public static float ZoomLimit(float dist, float min, float max)
    {
        if (dist < min)
            dist = min;
        if (dist > max)
            dist = max;

        return dist;
    }


}