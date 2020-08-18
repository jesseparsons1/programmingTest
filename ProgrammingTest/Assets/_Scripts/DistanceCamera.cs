using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCamera : MonoBehaviour
{
    [SerializeField]
    private float acceleration = 0;
    [SerializeField]
    private float deceleration = 0;
    [SerializeField]
    private float maxSpeed = 0;
    [SerializeField]
    private float maxZoom = 0;

    private void OnEnable() => transform.localPosition = Vector3.zero;

    private float curSpeed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            curSpeed = Mathf.Lerp(curSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            curSpeed = Mathf.Lerp(curSpeed, -maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, deceleration * Time.deltaTime);
        }

        transform.localPosition += Vector3.forward * curSpeed * Time.deltaTime;

        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, maxZoom);
    }
}
