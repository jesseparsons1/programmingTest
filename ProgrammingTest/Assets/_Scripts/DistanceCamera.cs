using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject cam = null;
    [SerializeField]
    private Transform rail = null;

    //Moving variables
    [SerializeField]
    private float moveSpeed = 0;
    [SerializeField]
    private List<Vector3> viewPlanetPositions = new List<Vector3>();

    //Zooming variables
    [SerializeField]
    private float zoomAcceleration = 0;
    [SerializeField]
    private float mouseWheelZoomAccelerationScale = 3f;
    [SerializeField]
    private float zoomDeceleration = 0;
    [SerializeField]
    private float maxZoomSpeed = 0;
    [SerializeField]
    private float zoomAdjustSpeed = 0;
    [SerializeField]
    private List<float> maxZoomLevels = new List<float>();
    [SerializeField]
    private float maxZoomOutLevel;
    
    private float curZoomSpeed;

    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private void OnEnable()
    {
        cam.transform.localPosition = Vector3.zero;
        GameManager.OnToggleModeEvent += OnToggleMode;
    }

    private void OnDisable()
    {
        GameManager.OnToggleModeEvent -= OnToggleMode;
    }

    private void OnToggleMode(bool switchingToSizeMode) => cam.SetActive(!switchingToSizeMode);

    private void Update()
    {
        if (!isMoving)
        {
            if (IsInputtingForward())
            {
                float lerpPercent = zoomAcceleration * (IsUsingOnlyMouseWheel() ? mouseWheelZoomAccelerationScale : 1);
                lerpPercent *= Time.deltaTime;

                curZoomSpeed = Mathf.Lerp(curZoomSpeed, maxZoomSpeed, lerpPercent);
            }
            else if (IsInputtingBackward())
            {
                float lerpPercent = zoomAcceleration * (IsUsingOnlyMouseWheel() ? mouseWheelZoomAccelerationScale : 1);
                lerpPercent *= Time.deltaTime;

                curZoomSpeed = Mathf.Lerp(curZoomSpeed, -maxZoomSpeed, lerpPercent);
            }
            else
            {
                float lerpPercent = zoomDeceleration;
                lerpPercent *= Time.deltaTime;

                curZoomSpeed = Mathf.Lerp(curZoomSpeed, 0, lerpPercent);
            }

            cam.transform.localPosition += Vector3.forward * curZoomSpeed * Time.deltaTime;
            float clamp = cam.transform.localPosition.z > 0 ? maxZoomLevels[GameManager.instance.CurrentlyViewedPlanetIndex] : maxZoomOutLevel;
            cam.transform.localPosition = Vector3.ClampMagnitude(cam.transform.localPosition, clamp);
        }
        else
        {
            curZoomSpeed = 0f;
        }
    }

    private bool IsInputtingForward() => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") < 0;
    private bool IsInputtingBackward() => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") > 0;
    private bool IsUsingOnlyMouseWheel() => (Input.GetAxis("Mouse ScrollWheel") != 0) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow);

    bool isMoving;

    public IEnumerator MoveToView(int planetIndex)
    {
        Vector3 targetPosition = viewPlanetPositions[planetIndex];

        isMoving = true;

        //If more zoomed in that next planet allows, need to zoom out
        if (cam.transform.localPosition.z > 0 && cam.transform.localPosition.magnitude > maxZoomLevels[planetIndex])
        {
            //Get target position as current position clamped to next max zoom
            Vector3 targetPos = Vector3.ClampMagnitude(cam.transform.localPosition, maxZoomLevels[planetIndex]);
            
            //Lerp towards target position
            while (!cam.transform.localPosition.IsApproximately(targetPos, 0.05f))
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, targetPos, zoomAdjustSpeed * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
        }

        //While rail is not sufficiently close to target position, lerp it towards it
        while (!rail.position.IsApproximately(targetPosition, 0.05f))
        {
            rail.position = Vector3.Lerp(rail.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return waitForEndOfFrame;
        }

        isMoving = false;

        GameManager.instance.CurrentlyViewedPlanetIndex = planetIndex;
    }
}
