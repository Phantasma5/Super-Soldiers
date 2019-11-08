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
    [SerializeField] private InputField inputTxtBox;
    [SerializeField] private GameObject chatLog;
    #endregion
    #region Variables
    [HideInInspector] private bool CallbacksSet = false;
    #endregion
    private void Update()
    {
        if (null != References.localPlayer && !CallbacksSet)
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
        if (aMax > 10000)
        {
            ammoTxt.text = "Infinite";
        }
    }
    public void ChatboxEnable()
    {
        inputTxtBox.gameObject.SetActive(true);
        inputTxtBox.Select();
        inputTxtBox.ActivateInputField();
    }
    public void SendChat(bool all)
    {
        string message = inputTxtBox.text;
        inputTxtBox.text = "";
        inputTxtBox.gameObject.SetActive(false);
        References.client.SendChat(all, 1, message);
    }
    public void UpdateChatLog(string message)
    {
        GameObject temp = new GameObject();
        temp.AddComponent<CanvasRenderer>();
        Text txt = temp.AddComponent<Text>();
        txt.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;//https://stackoverflow.com/questions/30873343/how-to-set-a-font-for-a-ui-text-in-unity-3d-programmatically
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(250,20);//width, height
        
        GameObject newMessage = Instantiate(temp, chatLog.transform);
        newMessage.GetComponent<Text>().text = message;
    }

}
