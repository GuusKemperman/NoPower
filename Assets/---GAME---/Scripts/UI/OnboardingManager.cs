using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
class OnboardingStep
{
    public string StepText;
}

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private List<OnboardingStep> onboardingSteps = new List<OnboardingStep>();
    [SerializeField] private TextMeshProUGUI textMesh = null;
    private int currentIndex = 0;
    private void Start()
    {
        DisplayStep(currentIndex);
        PowerManager.OnGainedPower += SkipFirst;
        PlayerTurretSpawning.OnPlacedTurret += SkipSecond;
    }
    private void OnDestroy()
    {
        PowerManager.OnGainedPower -= SkipFirst;
        PlayerTurretSpawning.OnPlacedTurret -= SkipSecond;
        
    }
    private void SkipFirst()
    {
        if (currentIndex != 0) return;
        currentIndex++;
        DisplayStep(currentIndex);
    }
    
    private void SkipSecond()
    {
        if (currentIndex !=1) return;
        currentIndex++;
        DisplayStep(currentIndex);    }

    private void DisplayStep(int i)
    {
        OnboardingStep currentStep = onboardingSteps[i];
        textMesh.text = currentStep.StepText;
    }
}
