using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public GameObject missileMotor;
    public GameObject missileHead;
    public GameObject target;

    public float blastRadius = 20; //m
    public float missileMotorForce = 10_000f;
    public float heatTransferCoefficient = 57.0f;
    public float aluminumSpecificHeatCapacity = 900.0f;
    public float missileMass = 45.21f; // KG
    public float fuelMass = 105.49f; // KG
    public float specificImpulse = 250; // s


    private float surfaceTemperature;
    private float fixedDeltaTime;
    private float burnRateSeconds; // KG/s
    private float burnRate; // KG/fixed_update_tick
    private Rigidbody rb;
    public Vector3 forceDirection;

    EnvironmentManager envManager;

    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        rb = gameObject.GetComponent<Rigidbody>();
        envManager = GameObject.FindGameObjectWithTag("Environment").GetComponent<EnvironmentManager>();
        surfaceTemperature = envManager.GetTemperatureAtAltitude(transform.position.y);
        burnRateSeconds = CalculateBurnRate(specificImpulse, missileMotorForce, envManager.gravity);
        burnRate = burnRateSeconds * Time.fixedDeltaTime;

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
            ApplyForceFromMotor();
        }
    }

    void ApplyForceFromMotor()
    {
        // Apply burn rate to missile fuel
        fuelMass -= burnRate;
        if (fuelMass < 0) { fuelMass = 0; }
        else
        {
            forceDirection = (target.transform.position - missileHead.transform.position).normalized;

            // Clamp the force direction
            rb.AddForceAtPosition(forceDirection * missileMotorForce, missileMotor.transform.position);
        }


    }

    float CalculateBurnRate(float specificImpulse, float thrust, float gravitationalAcceleration)
    {
        // mdot = F / (I_sp * g_0)
        // \dot{m} = \frac{F}{I_{sp} \times g_0}

        return thrust / (specificImpulse * gravitationalAcceleration);
    }
}
