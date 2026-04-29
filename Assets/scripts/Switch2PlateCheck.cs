using System.Collections;
using UnityEngine;

public class Switch2PlateCheck : MonoBehaviour
{
    [Header("Correct Item")]
    public string correctTag = "Switch2Item";

    [Header("Switch Requirement")]
    public LeverSwitch switch1;

    [Header("Bulb")]
    public Renderer bulbRenderer;
    public Material redMat;
    public Material greenMat;
    public Material orangeMat;

    [Header("Battery Lights")]
    public Renderer battery4;
    public Renderer battery3;
    public Renderer battery2;
    public Renderer battery1;
    public Material batteryOnMat;

    [Header("Final Unlock")]
    public Renderer finalBulb; // Light Bulb 2.001
    public Material finalGreenMat;
    public DoorNewController door;

    private int itemsOnPlate = 0;
    private bool correctItemOnPlate = false;
    private bool sequenceStarted = false;

    void Start()
    {
        SetRed();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(correctTag))
        {
            // Only show orange if switch is ON and no correct item yet
            if (switch1 != null && switch1.IsActivated() && !correctItemOnPlate)
            {
                SetOrange();
            }
            return;
        }

        // Correct item
        correctItemOnPlate = true;
        itemsOnPlate++;

        UpdateBulb();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(correctTag))
        {
            itemsOnPlate--;

            if (itemsOnPlate <= 0)
            {
                correctItemOnPlate = false;
            }
        }

        UpdateBulb();
    }

    void UpdateBulb()
    {
        if (switch1 == null || !switch1.IsActivated())
        {
            SetRed();
            return;
        }

        if (correctItemOnPlate)
        {
            SetGreen();

            if (!sequenceStarted)
            {
                sequenceStarted = true;
                StartCoroutine(BatterySequence());
            }
        }
        else
        {
            SetRed();
        }
    }

    IEnumerator BatterySequence()
    {
        yield return new WaitForSeconds(2f);
        SetBattery(battery4);

        yield return new WaitForSeconds(2f);
        SetBattery(battery3);

        yield return new WaitForSeconds(2f);
        SetBattery(battery2);

        yield return new WaitForSeconds(2f);
        SetBattery(battery1);

        //   FINAL STEP
        ActivateFinal();
    }

    void ActivateFinal()
    {
        // Turn final bulb green
        if (finalBulb != null && finalGreenMat != null)
        {
            finalBulb.material = finalGreenMat;
        }

        // Unlock door
        if (door != null)
        {
            door.UnlockDoor();
        }
    }

    void SetBattery(Renderer r)
    {
        if (r != null && batteryOnMat != null)
        {
            r.material = batteryOnMat;
        }
    }

    void SetRed()
    {
        if (bulbRenderer != null && redMat != null)
            bulbRenderer.material = redMat;
    }

    void SetGreen()
    {
        if (bulbRenderer != null && greenMat != null)
            bulbRenderer.material = greenMat;
    }

    void SetOrange()
    {
        if (bulbRenderer != null && orangeMat != null)
            bulbRenderer.material = orangeMat;
    }
}