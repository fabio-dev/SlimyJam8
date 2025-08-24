using UnityEngine;

public class AnimateOutlineParameters : MonoBehaviour
{
    // --- Softness Animation ---
    [Header("Softness Animation")]
    [Tooltip("The minimum value for the outline softness.")]
    public float minSoftness = 0.7f;
    [Tooltip("The maximum value for the outline softness.")]
    public float maxSoftness = 3.0f;
    [Tooltip("How fast the softness animation plays.")]
    public float softnessSpeed = 1.0f;

    // --- Thickness Animation ---
    [Header("Thickness Animation")]
    [Tooltip("The minimum value for the outline thickness.")]
    public float minThickness = 5.0f;
    [Tooltip("The maximum value for the outline thickness.")]
    public float maxThickness = 23.8f;
    [Tooltip("How fast the thickness animation plays.")]
    public float thicknessSpeed = 1.0f;

    // --- Animation Offset ---
    [Header("Animation Offset")]
    [Tooltip("Adds a delay to the thickness animation to de-synchronize it from the softness animation. A value of 3.14 will make them perfectly opposite.")]
    public float animationOffset = 1.5f;

    // A reference to the material we will be changing.
    private Material material;
    public float minDeformation = 1.0f;
    public float maxDeformation = 4.0f;
    public float deformationSpeed = 0.5f;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Use .material to create a unique instance for this object.
            material = renderer.material;
        }
        else
        {
            Debug.LogError("No Renderer found on this GameObject. Cannot animate material.", this);
            enabled = false;
        }
    }

    void Update()
    {
        if (material == null) return;

        // --- Softness Calculation ---
        float sinWaveSoftness = Mathf.Sin(Time.time * softnessSpeed);
        float tSoftness = (sinWaveSoftness + 1.0f) / 2.0f; // Remap from [-1, 1] to [0, 1]
        float currentSoftness = Mathf.Lerp(minSoftness, maxSoftness, tSoftness);
        material.SetFloat("_OutlineSoftness", currentSoftness);


        // --- Thickness Calculation ---
        // We add the offset to Time.time here. This shifts the start of the sine wave,
        // creating the "delay" or de-synchronization effect.
        float sinWaveThickness = Mathf.Sin(Time.time * thicknessSpeed + animationOffset);
        float tThickness = (sinWaveThickness + 1.0f) / 2.0f; // Remap from [-1, 1] to [0, 1]
        float currentThickness = Mathf.Lerp(minThickness, maxThickness, tThickness);
        material.SetFloat("_OutlineThickness", currentThickness);

        float sinWaveDeform = Mathf.Sin(Time.time * deformationSpeed);
        float tDeform = (sinWaveDeform + 1.0f) / 2.0f;
        float currentDeform = Mathf.Lerp(minDeformation, maxDeformation, tDeform);
        material.SetFloat("_DeformationStrength", currentDeform);
    }

    private void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }
}