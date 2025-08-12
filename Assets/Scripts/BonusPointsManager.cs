using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointsManager : MonoBehaviour
{
    public GameObject bonusUI;
    private int shotsTaken = 0;
    private bool bonusActive = false;

    [Range(0f, 1f)]
    public float activationChance = 0.25f;

    public void RegisterShot(bool wasBackboardShot)
    {
        if (bonusActive)
        {
            if (wasBackboardShot)
            {
                DeactivateBonus();
            }
            return;
        }

        if (Random.value < activationChance)
        {
            ActivateBonus();
        }
    }

    private void ActivateBonus()
    {
        bonusActive = true;
        bonusUI.SetActive(true);
        Debug.Log("Backboard Bonus Activated!");
    }

    private void DeactivateBonus()
    {
        bonusActive = false;
        shotsTaken = 0;
        bonusUI.SetActive(false);
        Debug.Log("Backboard Bonus Removed!");
    }

    public bool IsBonusActive()
    {
        return bonusActive;
    }
}