using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public bool MouseDown => Input.GetMouseButtonDown(0);
    public bool MouseHold => Input.GetMouseButton(0);
    public bool MouseUp => Input.GetMouseButtonUp(0);

}
