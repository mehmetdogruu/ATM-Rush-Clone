using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private InputManager _inputManager;
    private float _firstTouchX;
    public float forwardSpeed;
    private GameObject _player;
    private void Awake()
    {
        _inputManager = new InputManager();
    }

    private void Start()
    {
        _player = Collactable.instance.collectedMoney[0];
    }

    private void Update()
    {
        var moveVector = new Vector3(0, 0, forwardSpeed * Time.deltaTime);
        float difference;
        if (_inputManager.MouseDown)
        {
            _firstTouchX = Input.mousePosition.x;
        }
        else if (_inputManager.MouseHold)
        {
            var lastTouchX = Input.mousePosition.x;
            difference = lastTouchX - _firstTouchX;
            var slideVector = new Vector3(difference * Time.deltaTime, 0, 0);
            //slideVector.x = Mathf.Clamp(slideVector.x, -0.25f, 025f);
            //moveVector += new Vector3(difference * Time.deltaTime, 0, 0);
            _player.transform.localPosition += slideVector;
            _player.transform.localPosition = Vector3.MoveTowards(_player.transform.localPosition, new Vector3(Mathf.Clamp(_player.transform.position.x, -2, 2), _player.transform.localPosition.y, _player.transform.localPosition.z), Time.fixedDeltaTime * 4);
            _firstTouchX = lastTouchX;
        }
        transform.position += moveVector;
    }



}
