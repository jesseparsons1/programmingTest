using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject cam = null;
    public GameObject Camera => cam;
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
    private float maxZoomOutLevel = 0;

    //Panning variables
    [SerializeField]
    private float panAcceleration = 0;
    [SerializeField]
    private float panDeceleration = 0;
    [SerializeField]
    private float maxPanSpeed = 0;
    [SerializeField]
    private float panResetSpeed = 0;

    private float curPanSpeed;
    private float curZoomSpeed;

    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private void OnEnable() => GameManager.OnToggleModeEvent += OnToggleMode;

    private void OnDisable() => GameManager.OnToggleModeEvent -= OnToggleMode;

    //When the mode changes, camera sets itself active accordingly
    private void OnToggleMode(bool switchingToSizeMode) => cam.SetActive(!switchingToSizeMode);

    private void Update()
    {
        //If not changing view and in distance mode...  
        if (!isChangingView && GameManager.instance.CurMode == 0)
        {
            //... then respond to players zoom and pan input
            ZoomCamera();
            PanCamera();
        }
        else
        {
            //otherwise, cancel zoom and pan movement
            curZoomSpeed = 0f;
            curPanSpeed = 0f;
        }
    }

    private void ZoomCamera()
    {
        //Calculate zoom acceleration and target zoom speed based on player input
        float acceleration = zoomDeceleration;
        float targetSpeed = 0f;

        if (InputHandler.IsInputtingForward())
        {
            acceleration = zoomAcceleration * (InputHandler.IsUsingOnlyMouseWheel() ? mouseWheelZoomAccelerationScale : 1);
            targetSpeed = maxZoomSpeed;
        }
        else if (InputHandler.IsInputtingBackward())
        {
            acceleration = zoomAcceleration * (InputHandler.IsUsingOnlyMouseWheel() ? mouseWheelZoomAccelerationScale : 1);
            targetSpeed = -maxZoomSpeed;
        }

        //Lerp current zoom speed towards target zoom speed
        curZoomSpeed = Mathf.Lerp(curZoomSpeed, targetSpeed, acceleration * Time.deltaTime);

        //Apply speed
        cam.transform.localPosition += Vector3.forward * curZoomSpeed * Time.deltaTime;

        //Clamp to within zoom bounds
        float clamp = cam.transform.localPosition.z > 0 ? maxZoomLevels[GameManager.instance.CurrentlyViewedPlanetIndex] : maxZoomOutLevel;
        cam.transform.localPosition = Vector3.ClampMagnitude(cam.transform.localPosition, clamp);
    }

    private void PanCamera()
    {
        //Calculate pan acceleration and target pan speed based on player input
        float acceleration = panDeceleration;
        float targetSpeed = 0f;

        if (InputHandler.IsInputtingLeft())
        {
            acceleration = panAcceleration;
            targetSpeed = maxPanSpeed;
        }
        else if (InputHandler.IsInputtingRight())
        {
            acceleration = panAcceleration;
            targetSpeed = -maxPanSpeed;
        }

        //Lerp current pan speed towards target pan speed
        curPanSpeed = Mathf.Lerp(curPanSpeed, targetSpeed, acceleration * Time.deltaTime);

        //Apply speed as a change in rotation
        Vector3 newRot = rail.parent.localRotation.eulerAngles + new Vector3(0, curPanSpeed * Time.deltaTime, 0);
        rail.parent.localRotation = Quaternion.Euler(newRot);
    }

    bool isChangingView;

    public IEnumerator ChangeView(int planetIndex)
    {
        isChangingView = true;

        yield return StartCoroutine(ResetPanRotation());
        yield return StartCoroutine(AdjustZoomForNextPlanet(planetIndex));
        yield return StartCoroutine(MoveAlongRail(planetIndex));

        isChangingView = false;
    }

    private IEnumerator ResetPanRotation()
    {
        //Reset rotation back to identity
        float startingAngle = rail.parent.localRotation.eulerAngles.y.PositiveModulo(360);
        float targetAngle = (startingAngle > 180) ? 360 : 0;
        float curAngle = startingAngle;

        //If not in approx correct place...
        while (!curAngle.IsApproximately(targetAngle, 0.05f))
        {
            //... lerp current angle towards target angle
            curAngle = rail.parent.localRotation.eulerAngles.y;
            Vector3 newRot = Mathf.Lerp(curAngle, targetAngle, panResetSpeed * Time.deltaTime) * Vector3.up;
            rail.parent.localRotation = Quaternion.Euler(newRot);

            yield return waitForEndOfFrame;
        }

        //Snap to correct rotation
        rail.parent.localRotation = Quaternion.identity;

        //Reset parent
        rail.SetParent(transform);
    }

    private IEnumerator AdjustZoomForNextPlanet(int nextPlanetIndex)
    {
        //If more zoomed in that next planet allows, need to zoom out
        if (cam.transform.localPosition.z > 0 && cam.transform.localPosition.magnitude > maxZoomLevels[nextPlanetIndex])
        {
            //Get target position as current position clamped to next max zoom
            Vector3 targetPos = Vector3.ClampMagnitude(cam.transform.localPosition, maxZoomLevels[nextPlanetIndex]);

            //Lerp towards target position
            while (!cam.transform.localPosition.IsApproximately(targetPos, 0.05f))
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, targetPos, zoomAdjustSpeed * Time.deltaTime);

                yield return waitForEndOfFrame;
            }
        }
    }

    private IEnumerator MoveAlongRail(int nextPlanetIndex)
    {
        Vector3 targetRailPos = viewPlanetPositions[nextPlanetIndex];

        //While rail is not sufficiently close to target position...
        while (!rail.position.IsApproximately(targetRailPos, 0.05f))
        {
            //... lerp it towards it
            rail.position = Vector3.Lerp(rail.position, targetRailPos, moveSpeed * Time.deltaTime);
            yield return waitForEndOfFrame;
        }

        //Set parent ready for panning
        rail.SetParent(GameManager.instance.GetPlanet(nextPlanetIndex).CameraViewPivot);
    }
}
