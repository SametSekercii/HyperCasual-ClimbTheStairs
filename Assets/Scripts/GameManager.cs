using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGameStarted;
    public Transform PillarSpawnPoint;
    public Transform Player;
    public GameObject Buttons;
    public GameObject TapToStart;
    public GameObject LeveledUpPanel;
    public Text IncomeUpgradeValue;
    public Text PlayerIncomeLevel;
    public Text StaminaUpgradeValue;
    public Text PlayerStaminaLevel;
    public Text SpeedUpgradeValue;
    public Text PlayerSpeedLevel;
    public Text PlayerLeveledUP;
    
    int PillarCounter;
    Transform LastLayer;

    void Start()
    {
        
        isGameStarted = false;
        instance = this;
        PillarCounter = 0;
    }
    
    
    void Update()
    {
        PlayerIncomeLevel.text="LVL"+PlayerController.instance.GetIncomeLevel().ToString();
        IncomeUpgradeValue.text=ButtonProcess.Instance.GetIncomeUpgradeValue().ToString();
        PlayerStaminaLevel.text="LVL"+ PlayerController.instance.GetStaminaLevel().ToString();
        StaminaUpgradeValue.text=ButtonProcess.Instance.GetStaminaUpgradeValue().ToString();
        PlayerSpeedLevel.text="LVL"+PlayerController.instance.GetSpeedLevel().ToString();
        SpeedUpgradeValue.text=ButtonProcess.Instance.GetSpeedUpgradeValue().ToString();
        PlayerLeveledUP.text= "Congratulations! Level UP "+PlayerController.instance.GetPlayerLevel().ToString();
        PlayerController.instance.CheckHeight();
        if (!isGameStarted)
        {  
            return;
        }
        Buttons.SetActive(false);
        TapToStart.SetActive(false);
        

    }
   
    public void CreateLayer()
    {
        var CreatedLayer =ObjectPooler.Instance.GetLayers();
        if (CreatedLayer != null)
        {
            if (PillarCounter != 0)
            {
                CreatedLayer.transform.position = PillarSpawnPoint.position;
                CreatedLayer.transform.position+=new Vector3 (0,PillarSpawnPoint.position.y*PillarCounter,0);
                CreatedLayer.transform.rotation =Quaternion.Euler(0,PillarCounter*15,0);
                LastLayer = CreatedLayer.transform.GetChild(0);
                CreatedLayer.SetActive(true);
                PillarCounter++;
                PlayerController.instance.HeightUp();
                

            }
            else
            {
                CreatedLayer.transform.position = PillarSpawnPoint.position;
                LastLayer = CreatedLayer.transform.GetChild(0);
                CreatedLayer.SetActive(true);
                PillarCounter++;
                PlayerController.instance.HeightUp();
            }

        }

    }
    public int getPillarAmount()
    {
        return PillarCounter;
    }
    public Transform getTargetLayer()
    {
        return LastLayer;
    }

    
    public void PlayerLeveledUp()
    {
        Time.timeScale = 0f;
        ButtonProcess.isGamePaused = true;
        LeveledUpPanel.gameObject.SetActive(true); 
        return;

    }
    

}
