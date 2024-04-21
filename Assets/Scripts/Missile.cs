using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public float surfaceTemperature = 15.0f;
    public float h = 57.0f;  // Heat transfer coefficient 
    public float aluminumSpecificHeatCapacity = 900.0f;  // Specific heat capacity of aluminum
    public float missileMass = 2f;


    private float fixedDeltaTime;
    private Rigidbody rb;

    EnvironmentManager envManager;

    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        rb = gameObject.GetComponent<Rigidbody>();
        envManager = GameObject.FindGameObjectWithTag("Environment").GetComponent<EnvironmentManager>();
        surfaceTemperature = envManager.GetTemperatureAtAltitude(transform.position.y);

        rb.mass = missileMass;
    }

    void FixedUpdate()
    {
        float altitude = transform.position.y;
        float ambientTemp = envManager.GetTemperatureAtAltitude(altitude);

        // Newton's Law of Cooling
        float rate = -h * (surfaceTemperature - ambientTemp) / (rb.mass * aluminumSpecificHeatCapacity);
        surfaceTemperature += rate * fixedDeltaTime;

        rb.mass = missileMass;
    }
}
