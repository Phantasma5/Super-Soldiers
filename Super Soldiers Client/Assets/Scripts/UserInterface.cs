using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    #region References
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject fuelBar;
    [SerializeField] private Text ammoTxt;
    #endregion
    #region Variables
    [HideInInspector] private bool CallbacksSet = false;
    #endregion
    private void Update()
    {
        if(null != References.localPlayer && !CallbacksSet)
        {
            CallbacksSet = true;
            References.localStatSystem.AddCallback(StatSystem.StatType.Health, UpdateHealthbar);
            References.localStatSystem.AddCallback(StatSystem.StatType.Fuel, UpdateFuelbar);
        }
    }

    private void UpdateHealthbar(StatSystem.StatType aStat, float aOldValue, float aNewValue)
    {
        Vector3 sca;
        sca = healthBar.transform.localScale;
        sca.x = aNewValue / References.localStatSystem.GetMaxValue(aStat);
        healthBar.transform.localScale = sca;
    }
    private void UpdateFuelbar(StatSystem.StatType aStat, float aOldValue, float aNewValue)
    {
        Vector3 sca;
        sca = fuelBar.transform.localScale;
        sca.x = aNewValue / References.localStatSystem.GetMaxValue(aStat);
        fuelBar.transform.localScale = sca;
    }
    public void UpdateAmmo(int aCur, int aMax)
    {
        ammoTxt.text = (aCur + "/" + aMax);
        if(aMax > 10000)
        {
            ammoTxt.text = "Infinite";
        }
    }
}
