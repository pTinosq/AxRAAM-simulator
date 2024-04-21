using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public LayerMask affectedLayer;
    public float windSpeed = 4f;
    public Vector2 windDirection = Vector2.one;

    void FixedUpdate()
    {
        // Calculate the force based on wind speed and direction
        Vector2 force = windDirection.normalized * windSpeed;

        // Perform the overlap box check to find colliders within the designated area
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, affectedLayer);

        // Apply force to each collider that has a Rigidbody
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>(); // Try to get the Rigidbody component
            if (rb != null)
            {
                rb.AddForce(new Vector3(force.x, 0, force.y)); // Apply the force 
            }
        }
    }
}
