using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointsManager : MonoBehaviour
{
    public GameObject bonusUI;
    private int shotsTaken = 0;
    private bool bonusActive = false;

    public void RegisterShot(bool wasBackboardShot)
    {
        shotsTaken++;

        if (!bonusActive && shotsTaken >= 3)
        {
            ActivateBonus();
        }

        if (bonusActive && wasBackboardShot)
        {
            DeactivateBonus();
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