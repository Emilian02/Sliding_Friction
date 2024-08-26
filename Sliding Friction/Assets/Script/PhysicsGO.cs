using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGO : MonoBehaviour
{
    [SerializeField] bool isGravityOn;
    [SerializeField] float velocity;
    [SerializeField] float mass;
    [SerializeField] float frictionCoefficient = 0.5f;
    [SerializeField] float raycastLength;
    [SerializeField] PlaneAngle inclineAngle;


    float gravity = -9.81f;
    RaycastHit hit;

    private void FixedUpdate()
    {
        // Apply physics to calculate the new velocity
        ApplyPhysics();

        // Calculate the next position based on current velocity
        Vector3 moveDirection = new Vector3(0, -velocity * Time.fixedDeltaTime, 0);
        Vector3 nextPosition = transform.position + moveDirection;

        // Check for collisions and adjust position if necessary
        if (CheckCollision(nextPosition))
        {
            HandleCollision();
        }

        transform.position = nextPosition;

        Debug.Log("Velocity: " + velocity + "\nPosition: " + transform.position);
    }

    private bool CheckCollision(Vector3 newPosition)
    {
        // Cast the ray from the new position downward
        return Physics.Raycast(newPosition, -transform.up, out hit, raycastLength);
    }

    private void HandleCollision()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastLength))
        {
            // Move the object to the collision point plus the normal to stay on the plane
            Vector3 projectedPosition = (hit.point + hit.normal) * raycastLength;
            transform.position = projectedPosition;

            // Align the rotation of the object to match the plane’s angle
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = targetRotation;

            // Adjust velocity based on friction and collision normal
            Vector3 slideDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
            velocity = Vector3.Dot(slideDirection, transform.forward) * velocity;
        }
    }

    private void ApplyPhysics()
    {
        // Convert incline angle to radians
        float angleRad = inclineAngle.GetZAxisRotationAngle() * Mathf.Deg2Rad;

        // Calculate forces
        float gravitationalForce = mass * gravity;
        float gravitationalForceAlongPlane = gravitationalForce * Mathf.Sin(angleRad);
        float normalForce = gravitationalForce * Mathf.Cos(angleRad);
        float frictionForce = frictionCoefficient * normalForce;

        // Calculate net force and update velocity
        float netForce = gravitationalForceAlongPlane - frictionForce;
        float acceleration = netForce / mass;
        velocity += acceleration * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.up * raycastLength);
    }
}
