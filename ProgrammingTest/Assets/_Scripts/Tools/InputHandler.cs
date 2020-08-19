using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static bool IsInputtingForward() => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") < 0;
    public static bool IsInputtingBackward() => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") > 0;
    public static bool IsInputtingLeft() => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    public static bool IsInputtingRight() => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    public static bool IsUsingOnlyMouseWheel() => (Input.GetAxis("Mouse ScrollWheel") != 0) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow);
}
