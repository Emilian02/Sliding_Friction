using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGO : MonoBehaviour
{
    [SerializeField] bool isGravityOn;
    [SerializeField] float velocity;
    [SerializeField] float mass;
    [SerializeField] float raycastLength;


    float gravity = -9.81f;
    Vector3 moveDirection;
    bool isGrounded;
    bool wasGroundedLastFrame;

    private void Update()
    {
        CheckCollisions();
        if (isGravityOn && !isGrounded)
        {
            ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        // Calculate gravity force and update velocity
        float gravityForce = gravity * mass;
        velocity += gravityForce * Time.deltaTime;

        // Update position based on velocity
        moveDirection = new Vector3(0, velocity * Time.deltaTime, 0);
        transform.position += moveDirection;
    }

    private void CheckCollisions()
    {
        RaycastHit hit;
        // Cast the ray from the current position downward
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastLength))
        {
            // Check if the ray hit something
            if (hit.collider != null)
            {
                isGrounded = true;


                // Adjust position only if it was not grounded last frame
                if (!wasGroundedLastFrame)
                {
                    // Adjust position to be just above the surface
                    transform.position = new Vector3(transform.position.x, hit.point.y + raycastLength, transform.position.z);
                    velocity = 0.0f;
                }
            }
        }
        else
        {
            isGrounded = false;
        }

        // Update the grounded state
        wasGroundedLastFrame = isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * raycastLength);
    }
}
