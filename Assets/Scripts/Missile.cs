using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public GameObject missileMotor;

    public float missileMotorForce = 10_000f;
    public float heatTransferCoefficient = 57.0f;  // Heat transfer coefficient 
    public float aluminumSpecificHeatCapacity = 900.0f;  // Specific heat capacity of aluminum
    public float missileMass = 45.21f;
    public float fuelMass = 105.49f;

    private float surfaceTemperature;
    private float fixedDeltaTime;
    private Rigidbody rb;

    EnvironmentManager envManager;

    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        rb = gameObject.GetComponent<Rigidbody>();
        envManager = GameObject.FindGameObjectWithTag("Environment").GetComponent<EnvironmentManager>();
        surfaceTemperature = envManager.GetTemperatureAtAltitude(transform.position.y);

        rb.mass = missileMass + fuelMass;
    }

    void FixedUpdate()
    {
        float altitude = transform.position.y;
        float ambientTemp = envManager.GetTemperatureAtAltitude(altitude);

        // Newton's Law of Cooling
        float rate = -heatTransferCoefficient * (surfaceTemperature - ambientTemp) / (rb.mass * aluminumSpecificHeatCapacity);
        surfaceTemperature += rate * fixedDeltaTime;

        rb.mass = missileMass + fuelMass;

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space!");
            ApplyForceFromMotor();
        }
    }

    void ApplyForceFromMotor()
    {
        // Assuming the missile motor is at the back of the missile and force is applied forward
        Vector3 forceDirection = missileMotor.transform.forward;  // Direction of force
        rb.AddForceAtPosition(forceDirection * missileMotorForce, missileMotor.transform.position);
        Debug.Log("lesgo!");
    }
}
