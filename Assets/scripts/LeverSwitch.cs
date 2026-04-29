using System.Collections;
using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [Header("Lever Positions")]
    public Transform leverUp;
    public Transform leverDown;
    public float moveDuration = 0.4f;

    [Header("Bulb")]
    public Renderer bulbRenderer;
    public Material startMaterial;
    public Material warmMaterial;
    public Material doneMaterial;
    public float fadeSpeed = 2f;

    [Header("Water Wheel")]
    public WaterWheelSpin waterWheel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip leverSound;

    private bool activated = false;
    private bool isRunning = false;

    void Start()
    {
        if (bulbRenderer != null && startMaterial != null)
        {
            bulbRenderer.material = startMaterial;
        }

        if (leverUp != null)
        {
            transform.position = leverUp.position;
            transform.rotation = leverUp.rotation;
        }
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void ActivateLever()
    {
        if (!isRunning)
        {
            //  Play sound
            if (audioSource != null && leverSound != null)
                audioSource.PlayOneShot(leverSound);

            if (!activated)
                StartCoroutine(ActivateRoutine());
            else
                StartCoroutine(DeactivateRoutine());
        }
    }

    void OnMouseDown()
    {
        ActivateLever();
    }

    IEnumerator ActivateRoutine()
    {
        isRunning = true;
        activated = true;

        yield return StartCoroutine(MoveLever(leverDown));

        if (waterWheel != null)
            waterWheel.SetSpeed(0f);

        yield return StartCoroutine(FadeToMaterial(warmMaterial));

        if (waterWheel != null)
            waterWheel.SetSpeed(0.5f);

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(FadeToMaterial(doneMaterial));

        if (waterWheel != null)
            waterWheel.SetSpeed(1f);

        isRunning = false;
    }

    IEnumerator DeactivateRoutine()
    {
        isRunning = true;
        activated = false;

        yield return StartCoroutine(MoveLever(leverUp));

        if (bulbRenderer != null && startMaterial != null)
        {
            yield return StartCoroutine(FadeToMaterial(startMaterial));
        }

        if (waterWheel != null)
        {
            waterWheel.SetSpeed(0f);
        }

        isRunning = false;
    }

    IEnumerator MoveLever(Transform target)
    {
        if (target == null)
            yield break;

        float t = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    IEnumerator FadeToMaterial(Material targetMaterial)
    {
        if (bulbRenderer == null || targetMaterial == null)
            yield break;

        Material currentMat = bulbRenderer.material;
        Color startColor = currentMat.color;
        Color targetColor = targetMaterial.color;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            bulbRenderer.material.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        bulbRenderer.material = targetMaterial;
    }
}