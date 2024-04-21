using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Atmospheric Settings")]
    public float gravity = 9.8076f;

    [Header("Default Atmospheric Conditions")]
    public float seaLevelTemperatureC = 15;

    private List<AtmosphericLayer> atmosphericLayers;

    void Awake()
    {
        // Set global gravity
        Physics.gravity = new Vector3(0, -gravity, 0);

        // Create ELR Levels (ISA)
        atmosphericLayers = new() {
            new AtmosphericLayer { baseAltitude = 0f,     ceilAltitude=10999f,  lapseRate = -6.5f, baseTemperature = seaLevelTemperatureC }, // Troposphere
            new AtmosphericLayer { baseAltitude = 11000f, ceilAltitude=19999,   lapseRate = 0f,    baseTemperature = -56.5f               }, // Tropopause
            new AtmosphericLayer { baseAltitude = 20000f, ceilAltitude=31999,   lapseRate = 1f,    baseTemperature = -56.5f               }, // Stratosphere
            new AtmosphericLayer { baseAltitude = 32000f, ceilAltitude=46999,   lapseRate = 2.8f,  baseTemperature = -44.5f               }, // Stratosphere
            new AtmosphericLayer { baseAltitude = 47000f, ceilAltitude=50999,   lapseRate = 0f,    baseTemperature = -2.5f                }, // Stratopause
            new AtmosphericLayer { baseAltitude = 51000f, ceilAltitude=70999,   lapseRate = -2.8f, baseTemperature = -2.5f                }, // Mesosphere
            new AtmosphericLayer { baseAltitude = 71000f, ceilAltitude=84851f,  lapseRate = -2f,   baseTemperature = -58.5f               }, // Mesosphere
            new AtmosphericLayer { baseAltitude = 84852f, ceilAltitude=500000f, lapseRate = 0f,    baseTemperature= -86.20f               } // Mesopause
        };
    }

    public class AtmosphericLayer
    {
        public float baseAltitude;
        public float ceilAltitude;
        public float lapseRate;
        public float baseTemperature;
    }


    public float GetTemperatureAtAltitude(float altitude)
    {
        // Adjust altitude for calculation (incoming altitude is in unit of 0.1m)
        altitude *= 10;

        AtmosphericLayer currentLayer = atmosphericLayers[0];

        // Find current atmospheric layer
        foreach (var layer in atmosphericLayers)
        {
            if (altitude >= layer.baseAltitude && altitude <= layer.ceilAltitude)
            {
                currentLayer = layer;
            }
        }

        // Claculate ambient air temperature using the ISA's values
        float temperature = currentLayer.baseTemperature + (altitude - currentLayer.baseAltitude) * 0.001f * currentLayer.lapseRate;

        return temperature;
    }
}
