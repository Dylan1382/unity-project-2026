using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    [Header("Flow Settings")]
    public float flowSpeed = 0.2f;

    [Header("Wave Settings")]
    public float waveStrength = 0.05f;   // small distortion
    public float waveSpeed = 1.5f;

    [Header("Secondary Wave (breaks uniform look)")]
    public float waveStrength2 = 0.03f;
    public float waveSpeed2 = 2.3f;

    private Renderer rend;
    private Vector2 baseOffset;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Set tiling (you can tweak this)
        rend.material.mainTextureScale = new Vector2(3, 3);

        baseOffset = Vector2.zero;
    }

    void Update()
    {
        float time = Time.time;

        // Main flow (river direction)
        float flow = time * flowSpeed;

        // Wave distortion (Y direction wobble)
        float wave1 = Mathf.Sin(time * waveSpeed) * waveStrength;

        // Secondary wave (adds variation so it’s not uniform)
        float wave2 = Mathf.Cos(time * waveSpeed2) * waveStrength2;

        // Combine everything
        float finalY = wave1 + wave2;

        // Apply texture movement
        rend.material.mainTextureOffset = new Vector2(-flow, finalY);
    }
}