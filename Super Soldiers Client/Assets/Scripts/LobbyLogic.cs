using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyLogic : MonoBehaviour
{
    public Dropdown weaponSelect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VoteTeamGame()
    {
        ExampleClient.GetInstance().ReadyUpTeamGame();
    }

    public void VoteFFA()
    {
        ExampleClient.GetInstance().ReadyUpFFA();
    }

    public void ValueChanged()
    {
        ExampleClient.GetInstance().weapon = weaponSelect.value;
    }
}
