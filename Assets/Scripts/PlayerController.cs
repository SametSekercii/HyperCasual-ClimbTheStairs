using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum PlayerState
{
    Moving,
    Idle
}

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;
    private Material PlayerMaterial { get { return transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material; } }
    public Transform PillarSpawnPoint;
    public Transform ScoreTable;
    public Text ScoreTableText;
    public GameObject sweatingPrefab;
    public Transform[] sweatingPoints;
    public Text MoneyText;
    public Text PlayerLevelText;
    float elapsedTime = 0;
    private float Height;
    public float PlayerMaxStamina=100;
    public float PlayerStamina;
    private int PlayerIncomeLevel=1,PlayerStaminaLevel=1,PlayerSpeedLevel=1;
    private float PlayerWealth=0;
    private float ElapsedTime;
    private int PlayerLevel=1;
    GameObject SweatingEffect;
    Vector3 TargetPosition;
    PlayerState playerState;
    delegate void TurnDelegate();
    TurnDelegate turnDelegate;
    Animator animator { get { return GetComponent<Animator>(); } }
    void Awake()
    {
        #region PLATFORM FOR TURNING
        #if UNITY_EDITOR
                turnDelegate = PlayKeyboard;
        #endif

        #if UNITY_ANDROID
                turnDelegate = PlayTouchScreen;
        #endif
        #endregion
        if (PlayerPrefs.HasKey("PlayerMaxStamina"))
        {
            PlayerMaxStamina = PlayerPrefs.GetFloat("PlayerMaxStamina");
        }
        if (PlayerPrefs.HasKey("PlayerWealth"))
        {
            PlayerWealth = PlayerPrefs.GetFloat("PlayerWealth"); ;
        }
        if (PlayerPrefs.HasKey("PlayerIncomeLevel"))
        {
            PlayerIncomeLevel = PlayerPrefs.GetInt("PlayerIncomeLevel"); ;
        }
        if (PlayerPrefs.HasKey("PlayerStaminaLevel"))
        {
            PlayerStaminaLevel = PlayerPrefs.GetInt("PlayerStaminaLevel"); ;
        }
        if (PlayerPrefs.HasKey("PlayerSpeedLevel"))
        {
            PlayerSpeedLevel = PlayerPrefs.GetInt("PlayerSpeedLevel"); ;
        }
        if (PlayerPrefs.HasKey("Height"))
        {
            Height = PlayerPrefs.GetFloat("Height");
        }
        if (PlayerPrefs.HasKey("PlayerLevel"))
        {
            PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");
        }
        HeightDown();
        PlayerStamina = PlayerMaxStamina; 
        instance = this;
        playerState = PlayerState.Idle; 
             
    }

   
   

    void Update()
    {
        
        ScoreTableText.text = Height.ToString("0.0") + "m";
        MoneyText.text = PlayerWealth.ToString("0.0");
        PlayerLevelText.text = "LEVEL"+PlayerLevel.ToString();
        if (!GameManager.isGameStarted)
        {
            return;
        }
        Debug.Log("Stamina= "+PlayerStamina);
        turnDelegate();
        CheckStamina();     
        if (Vector3.Distance(transform.position,TargetPosition)<0.01f) playerState = PlayerState.Idle;
        if (playerState == PlayerState.Moving)
        {
            animator.SetBool("isMoving", true);
        }
        if (playerState == PlayerState.Idle)
        {
            animator.SetBool("isMoving", false);
        }
        
    }
    
    IEnumerator move()
    {
        while(transform.position!= TargetPosition)
        {
            playerState = PlayerState.Moving;
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Time.deltaTime*0.5f);
            yield return null;
        }
    }
   
    void SetLookRotation()
    {
        Quaternion lookRotation = new Quaternion();
        lookRotation = Quaternion.LookRotation(-GameManager.instance.getTargetLayer().right);
        while (transform.rotation != lookRotation)
        {
            transform.Rotate(transform.up,2f);
        } 
    }
    public void PlayerLevelUp()
    {
        PlayerLevel++;
        PlayerPrefs.SetInt("PlayerLevel",PlayerLevel);
        GameManager.instance.PlayerLeveledUp();

    }

    void CheckStamina()
    {
        PlayerMaterial.color = new Color(1f, PlayerStamina * 7f / 255f, PlayerStamina * 7f / 255f);
        if (PlayerStamina <= 0)
        {
            PlayerMaterial.color=Color.white;
            gameObject.SetActive(false);
            ButtonProcess.Instance.RestartGame();
        }
        if (PlayerStamina < 60)
        {
            if ((ElapsedTime += Time.deltaTime) > PlayerStamina / PlayerMaxStamina)
            {
                for (int i = 0; i < sweatingPoints.Length; i++)
                {
                    SweatingEffect = ObjectPooler.Instance.GetSweating();
                    if (SweatingEffect != null)
                    {
                        SweatingEffect.transform.position = sweatingPoints[i].position;
                        SweatingEffect.transform.rotation = sweatingPoints[i].rotation;
                        SweatingEffect.SetActive(true);

                    }
                }
                ElapsedTime = 0f;
            }
        }

    }

    void GainMoney()
    {
        PlayerWealth += PlayerIncomeLevel * 10f;
        PlayerPrefs.SetFloat("PlayerWealth", PlayerWealth);
    }

    
    public bool UpgradeIncome()
    {
        float NeededMoney=ButtonProcess.Instance.GetIncomeUpgradeValue();
        if (PlayerWealth > NeededMoney)
        {
            PlayerIncomeLevel++;
            PlayerWealth -= NeededMoney;
            PlayerPrefs.SetFloat("PlayerWealth", PlayerWealth);
            PlayerPrefs.SetInt("PlayerIncomeLevel", PlayerIncomeLevel);
            return true;

        }
        return false;

    }
    public bool UpgradeStamina()
    {
        float NeededMoney= ButtonProcess.Instance.GetStaminaUpgradeValue();
        if(PlayerWealth > NeededMoney)
        {
            PlayerStaminaLevel++;
            PlayerMaxStamina += 20;
            PlayerWealth -= NeededMoney;
            PlayerPrefs.SetFloat("PlayerWealth", PlayerWealth);
            PlayerPrefs.SetInt("PlayerStaminaLevel", PlayerStaminaLevel);
            PlayerPrefs.SetFloat("PlayerMaxStamina",PlayerMaxStamina);
            return true;

        }
        return false;
    }
    public bool UpgradeSpeed()
    {
        float NeededMoney = ButtonProcess.Instance.GetSpeedUpgradeValue();
        if (PlayerWealth > NeededMoney)
        {
            PlayerSpeedLevel++;       
            PlayerWealth -= NeededMoney;
            PlayerPrefs.SetFloat("PlayerWealth", PlayerWealth);
            PlayerPrefs.SetInt("PlayerSpeedLevel", PlayerSpeedLevel);
            return true;

        }
        return false;
    }
    public int GetIncomeLevel()
    {
        return PlayerIncomeLevel;
    }
    public int GetStaminaLevel()
    {
        return PlayerStaminaLevel;
    }
    public int GetSpeedLevel()
    {
        return PlayerSpeedLevel;
    }
    public void HeightUp()
    {
        Height += 1f;
    }
    public int GetPlayerLevel()
    {
        return PlayerLevel;
    }
    public void HeightDown()
    {
        Height = PlayerLevel * 100 - 100;
        PlayerPrefs.SetFloat("Height", Height);
    }
    private void PlayTouchScreen()
    {
        if ((elapsedTime += Time.deltaTime) > (1 - PlayerSpeedLevel * 0.07f))
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                GameManager.instance.CreateLayer();
                TargetPosition = GameManager.instance.getTargetLayer().position + new Vector3(0, 0.1f, 0);
                StartCoroutine(move());
                SetLookRotation();
                Handheld.Vibrate();
                GainMoney();
                PlayerPrefs.SetFloat("Height", Height);
                ScoreTable.position += new Vector3(0, PillarSpawnPoint.position.y * 1, 0);
                PlayerStamina -= PlayerLevel * 3;
                Debug.Log(PlayerStamina);
            }
            elapsedTime = 0;
        }
    }
    private void PlayKeyboard()
    {
        if((elapsedTime += Time.deltaTime) > (1 - PlayerSpeedLevel * 0.07f))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GameManager.instance.CreateLayer();
                TargetPosition = GameManager.instance.getTargetLayer().position + new Vector3(0, 0.1f, 0);
                StartCoroutine(move());
                SetLookRotation();
                GainMoney();        
                PlayerPrefs.SetFloat("Height", Height);
                ScoreTable.position += new Vector3(0, PillarSpawnPoint.position.y * 1, 0);
                PlayerStamina -= PlayerLevel * 3;
                Debug.Log(PlayerStamina);
            }
            elapsedTime = 0;
        }
       
    }
    public void CheckHeight()
    {
        if(Height >= 100 * PlayerLevel)
        {
            PlayerLevelUp();
        }
    }
}
