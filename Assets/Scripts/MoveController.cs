using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private float _inputDirection;      //x component of _moveVector
    [SerializeField]
    private float _verticalVelocity;    //y component of _moveVector
    [SerializeField]
    private float _speed = 5f;       //Speed multiplier for movement
    [SerializeField]
    private float _gravity = -30f;       //TODO: Customize gravity using g = (-2 * peakHeight * yVelocity * yVelocity)/(xDistance * xDistance);
    private float _jumpForce = 10f;     //velocity for jump
    private bool _canDoubleJump = false;
    private Vector3 _moveVector;
    private Vector3 _lastVector;
    private CharacterController _controller;
    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _moveVector = new Vector3(0, 0.001f, 0);
        _inputDirection = Input.GetAxisRaw("Horizontal");
        //Debug.Log(_controller.isGrounded);

        if(ControllerGrounded())
        {
            _verticalVelocity = 0;
            
            //Player Jump
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _verticalVelocity = 10f;
                _canDoubleJump = true;
            }
            _moveVector.x = _inputDirection;  //might get rid of
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(_canDoubleJump == true)
                {
                    _verticalVelocity = _jumpForce;
                    _canDoubleJump = false;
                }

            }

            _verticalVelocity += _gravity * Time.deltaTime;
            _moveVector.x = _lastVector.x; //might delete
        }
        
        
        _moveVector.y = _verticalVelocity;      //might get rid fo this caching for gamefeel

        //_moveVector = new Vector3(_inputDirection, _verticalVelocity, 0);
        _controller.Move(_moveVector * _speed * Time.deltaTime);
        _lastVector = _moveVector;
    }

    private bool ControllerGrounded()
    {
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = _controller.bounds.center;
        rightRayStart = _controller.bounds.center;

        leftRayStart.x -= _controller.bounds.extents.x;
        rightRayStart.x += _controller.bounds.extents.x;
        
        Debug.DrawRay(leftRayStart, Vector3.down, Color.blue);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.red);

        if(Physics.Raycast(leftRayStart, Vector3.down, (_controller.height/2)+ 0.2f))
        {
            return true;
        }
        if(Physics.Raycast(rightRayStart, Vector3.down, (_controller.height/2)+ 0.2f))
        {
            return true;
        }
        return false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);

        if(_controller.collisionFlags == CollisionFlags.Sides)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 2f);
                _moveVector = hit.normal * _speed;
                _moveVector.y = _jumpForce;
                _canDoubleJump = true;
            }
            
        }
    }


}