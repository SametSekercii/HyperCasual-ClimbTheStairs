using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonProcess : MonoBehaviour
{
    public static ButtonProcess Instance { get; private set; }
    float IncomeUpgradeValue=50f;
    float IncomeUpgradeValueCounter=0;
    float StaminaUpgradeValue=50f;
    float StaminaUpgradeValueCounter=0;
    float SpeedUpgradeValue = 50f;
    float SpeedUpgradeValueCounter=0;
    public static bool isGamePaused=false;
   
    public GameObject PausePanel;


    void Start()
    {
       
        if (PlayerPrefs.HasKey("IncomeUpgradeValue"))
        {
            IncomeUpgradeValue = PlayerPrefs.GetFloat("IncomeUpgradeValue");
        }
        if (PlayerPrefs.HasKey("IncomeUpgradeValueCounter"))
        {
            IncomeUpgradeValueCounter = PlayerPrefs.GetFloat("IncomeUpgradeValueCounter"); ;
        }
        if (PlayerPrefs.HasKey("StaminaUpgradeValue"))
        {
            StaminaUpgradeValue = PlayerPrefs.GetFloat("StaminaUpgradeValue"); ;
        }
        if (PlayerPrefs.HasKey("StaminaUpgradeValueCounter"))
        {
            StaminaUpgradeValueCounter = PlayerPrefs.GetFloat("StaminaUpgradeValueCounter"); ;
        }
        if (PlayerPrefs.HasKey("SpeedUpgradeValue"))
        {
            SpeedUpgradeValue = PlayerPrefs.GetFloat("SpeedUpgradeValue"); ;
        }
        if (PlayerPrefs.HasKey("SpeedUpgradeValueCounter"))
        {
            SpeedUpgradeValueCounter = PlayerPrefs.GetFloat("SpeedUpgradeValueCounter"); ;
        }

        Instance = this;
         
    }

    
    void Update()
    {
       
    }

    public void UpgradeIncome()
    {
        if (PlayerController.instance.UpgradeIncome())
        {
            IncomeUpgradeValue += IncomeUpgradeValueCounter * 15 + IncomeUpgradeValue;
            IncomeUpgradeValueCounter++;
            PlayerPrefs.SetFloat("IncomeUpgradeValue", IncomeUpgradeValue);
            PlayerPrefs.SetFloat("IncomeUpgradeValueCounter", IncomeUpgradeValueCounter);
        }
    }
    public void UpgradeStamina()
    {
        if (PlayerController.instance.UpgradeStamina())
        {
            StaminaUpgradeValue += IncomeUpgradeValueCounter * 15 + StaminaUpgradeValue;
            StaminaUpgradeValueCounter++;
            PlayerPrefs.SetFloat("StaminaUpgradeValue", StaminaUpgradeValue);
            PlayerPrefs.SetFloat("StaminaUpgradeValueCounter", StaminaUpgradeValueCounter);
        }
    }
    public void UpgradeSpeed()
    {
        if (PlayerController.instance.UpgradeSpeed())
        {
            SpeedUpgradeValue += SpeedUpgradeValueCounter * 15 + SpeedUpgradeValue;
            SpeedUpgradeValueCounter++;
            PlayerPrefs.SetFloat("SpeedUpgradeValue", SpeedUpgradeValue);
            PlayerPrefs.SetFloat("SpeedUpgradeValueCounter", SpeedUpgradeValueCounter);
        }
    }
    public void StartGame()
    {
        GameManager.isGameStarted = true;
    }
    public float GetIncomeUpgradeValue()
    {
        return IncomeUpgradeValue;
    }
    public float GetStaminaUpgradeValue()
    {
        return StaminaUpgradeValue;
    }
    public float GetSpeedUpgradeValue()
    {
        return SpeedUpgradeValue;
    }
    public void PauseAndContinueButton()
    {
        if (!isGamePaused)
        {
            
            Time.timeScale = 0f;
            PausePanel.gameObject.SetActive(true);
            isGamePaused = true;
            return;
        }
        if (isGamePaused)
        {
            
            Time.timeScale = 1f;
            PausePanel.gameObject.SetActive(false);
            isGamePaused = false;
            return;
        }
    }
    public void RestartGame()
    {
        PlayerController.instance.HeightDown();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        PausePanel.gameObject.SetActive(false);
        isGamePaused = false;
        return;

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

}
