using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HovercraftMovement : MonoBehaviour
{
    Rigidbody rb;
    InputController input;

    [SerializeField] float thrust;
    [SerializeField] float turningSpeed;
    [SerializeField] float dragMultiplier;
    [SerializeField] float turningForce;
    [SerializeField] float gravityMultiplier;
    [SerializeField] float hoverHeight;
    [SerializeField] float rollCorrectionTorque;
    [SerializeField] float rollAngleSmootingPoint;
    [SerializeField] float pitchCorrectionTorque;
    [SerializeField] float pitchAngleSmootingPoint;
    [SerializeField] float brakingDragMultiplier;
    [SerializeField] float brakingTurnMultiplier;
    [SerializeField] float brakingTurnForceMultiplier;

    [SerializeField] Transform hovercraftFront;
    [SerializeField] Transform hovercraftRear;
    [SerializeField] Transform hovercraftLeftSide;
    [SerializeField] Transform hovercraftRightSide;

    float hovercraftLength;
    float hovercraftWidth;

    float steering;
    float throttle;
    float brake;

    float turningAmount;
    Vector3 rotation;
    Vector3 gravityDirection;

    void Awake()
    {
        input = GetComponent<InputController>();     
        rb = GetComponent<Rigidbody>();

        rotation = transform.rotation.eulerAngles;
        hovercraftLength = (hovercraftFront.position - hovercraftRear.position).magnitude;
        hovercraftWidth = (hovercraftLeftSide.position - hovercraftRightSide.position).magnitude;
        gravityDirection = -transform.up;
    }

    void Update()
    {
        steering = input.steering;
        throttle = input.throttle;
        brake = input.brake;
    }
    
    void FixedUpdate()
    {
        float speed = rb.velocity.magnitude;
        float currentDragMultiplier = dragMultiplier;
        float currentTurningSpeed = turningSpeed;
        float currentTurningForce = turningForce;

        // Braking
        currentDragMultiplier *= 1 + input.brake * brakingDragMultiplier; 
        currentTurningSpeed *= 1 + input.brake * brakingTurnMultiplier;
        currentTurningForce *= 1 + input.brake * brakingTurnForceMultiplier;

        // Thrust
        rb.AddForce(transform.forward * thrust * throttle * Time.deltaTime);

        // Turning torque
        rb.AddTorque(transform.up * steering * currentTurningSpeed * Time.deltaTime);

        // Aerodynamic drag
        rb.AddForce(-rb.velocity * speed * currentDragMultiplier * Time.deltaTime);

        // Turning force
        rb.AddForce((transform.forward - rb.velocity.normalized) * speed * currentTurningForce * Time.deltaTime);

        // Angular drag from rigidbody

        // GRAVITY CALCULATION

        // Front raycast
        RaycastHit frontHit;
        float frontDistance = 0;
        if (Physics.Raycast(hovercraftFront.position, hovercraftFront.TransformDirection(-Vector3.up), out frontHit, Mathf.Infinity))
        {
            frontDistance = frontHit.distance;
        }

        // Rear raycast
        RaycastHit rearHit;
        float rearDistance = 0;
        if (Physics.Raycast(hovercraftRear.position, hovercraftRear.TransformDirection(-Vector3.up), out rearHit, Mathf.Infinity))
        {
            rearDistance = rearHit.distance;
        }

        float distanceDelta = frontDistance - rearDistance;
        float gravityAngle = Mathf.Tan(distanceDelta / hovercraftLength);
        gravityDirection = -transform.up;

        if (rearDistance != 0 && frontDistance != 0)
        {
            gravityDirection = Vector3.RotateTowards(-transform.up, -transform.forward, Mathf.Deg2Rad * gravityAngle, 0f);
        }

        // Hover if close to ground
        if (rearDistance < hoverHeight && frontDistance < hoverHeight)
            gravityDirection = -gravityDirection;


        float distanceDifferenceFront = Mathf.Abs(hoverHeight - frontDistance);
        float distanceDifferenceRear = Mathf.Abs(hoverHeight - rearDistance);
        if (distanceDifferenceFront < 1f && distanceDifferenceRear < 1f)
            gravityDirection *= (distanceDifferenceFront + distanceDifferenceRear) / 2;

        rb.AddForce(gravityDirection * gravityMultiplier * rb.mass * Time.deltaTime);


        // CORRECT PITCH

        float distanceDeltaPitch = frontDistance - rearDistance;
        float pitchAngle = Mathf.Tan(distanceDeltaPitch / hovercraftLength);

        float pitchMultiplier = 1;
        if (Mathf.Abs(pitchAngle) < pitchAngleSmootingPoint)
        {
            pitchMultiplier *= Mathf.Abs(pitchAngle) / pitchAngleSmootingPoint;
        }

        if (pitchAngle < 0)
            pitchMultiplier *= -1;

        rb.AddTorque(transform.right * pitchMultiplier * pitchCorrectionTorque * Time.deltaTime);


        // CORRECT ROLL (same as roll)

        // Left raycast
        RaycastHit leftHit;
        float leftDistance = 0;
        if (Physics.Raycast(hovercraftLeftSide.position, hovercraftLeftSide.TransformDirection(-Vector3.up), out leftHit, Mathf.Infinity))
        {
            leftDistance = leftHit.distance;
        }

        // Right raycast
        RaycastHit rightHit;
        float rightDistance = 0;
        if (Physics.Raycast(hovercraftRightSide.position, hovercraftRightSide.TransformDirection(-Vector3.up), out rightHit, Mathf.Infinity))
        {
            rightDistance = rightHit.distance;
        }

        float distanceDeltaRoll = rightDistance - leftDistance;
        float rollAngle = Mathf.Tan(distanceDeltaRoll / hovercraftWidth);

        float rollMultiplier = 1;
        if(Mathf.Abs(rollAngle) < rollAngleSmootingPoint)
        {
            rollMultiplier *= Mathf.Abs(rollAngle) / rollAngleSmootingPoint;
        }


        if (rollAngle > 0)
            rollMultiplier *= -1;

        rb.AddTorque(transform.forward * rollMultiplier * rollCorrectionTorque * Time.deltaTime);
    }
}