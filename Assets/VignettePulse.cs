using System;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Math = Unity.Mathematics.Geometry.Math;

public class VignettePulse : MonoBehaviour
{
    public ClampedFloatParameter intensity;
    public float intensityMin, intensityMax, current;
    public float pulseSpeed;

    public float timeInVolume;
    private float timeEntered;
    public float maxTimeInVolume;
    public float smoothnessMin, smoothnessMax;
    public float  weightTimeMax, weightMin;
    private float weightTime, weightCurrent;
    
    private Volume volume;

    private Vignette vignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current = 0f;
        //intensityMin = 0f;
        //intensityMax = 0.6f;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        intensity = vignette.intensity;
        intensity.overrideState = true;
        vignette.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        intensity.value = Mathf.PingPong(current, intensityMax - intensityMin) + intensityMin;
        current += (1f/pulseSpeed) * Time.deltaTime;

        timeInVolume = Mathf.Clamp(timeInVolume, 0, maxTimeInVolume);
        weightCurrent = Mathf.Clamp(weightCurrent, 0f, 1f);
        vignette.smoothness.value = Mathf.SmoothStep(smoothnessMin, smoothnessMax, timeInVolume/maxTimeInVolume);
        volume.weight = Mathf.SmoothStep(weightMin, 1f, timeInVolume/weightTimeMax);


    }

    private void FixedUpdate()
    {
        timeInVolume -= Time.fixedDeltaTime;
        weightCurrent -= Time.fixedDeltaTime * 0.5f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timeInVolume += Time.fixedDeltaTime * 2f;
            weightCurrent += Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        timeEntered = Time.time;
    }
}
