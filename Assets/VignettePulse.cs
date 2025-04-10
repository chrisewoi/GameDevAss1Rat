using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignettePulse : MonoBehaviour
{
    public ClampedFloatParameter intensity, intensityMax;
    private ClampedFloatParameter intensityMin;
    public float pulseSpeed;
    public Volume volume;

    public Vignette vignette;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        intensity = vignette.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
