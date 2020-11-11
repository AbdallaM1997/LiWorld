using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPlayer : MonoBehaviour
{
    public List<GameObject> playersPrefabs = new List<GameObject>();
    public List<Transform> instantiatePoints = new List<Transform>();
    public int instantiatePoint { get; private set; }
    public GameObject camera;
    public SceneFader fader;

    public GameObject buyButton;
    public GameObject selectButton;
    public GameObject lights;

    private GameObject playerobj;
    private int spwanIndex = 0;
   
    [HideInInspector]
    public GameObject player;

    int avilableCharater;

    #region Singleton 
    public static SelectPlayer instance;
    void Awake()
    {
        instance = this;
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion
    
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {

            avilableCharater = PlayerPrefs.GetInt(playerobj.name);
            CheckButton(avilableCharater, buyButton, selectButton, lights);
        }
    }

    private void CheckButton(int isAvilabeCharater, GameObject buyButton, GameObject openButton, GameObject lights)
    {
        if (isAvilabeCharater != 1)
        {
            buyButton.SetActive(true);
            openButton.SetActive(false);
            lights.SetActive(false);
        }
        else
        {
            buyButton.SetActive(false);
            openButton.SetActive(true);
            lights.SetActive(true);
        }
    }

    public void UseKeyForCharater(int amountUse)
    {
        if (CoinSystem.instance.savedKeys >= amountUse)
        {
            CoinSystem.instance.savedKeys -= amountUse;
            PlayerPrefs.SetInt("Keys", CoinSystem.instance.savedKeys);
            PlayerPrefs.SetInt(playerobj.name, 1);
            CoinSystem.instance.keyText.text = CoinSystem.instance.savedKeys.ToString();
        }
        else
        {
            CoinSystem.instance.anim.SetTrigger("Anim");
            CoinSystem.instance.alertText.text = "Not Have enough Keys";
        }
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            instantiatePoint = 0;
        }else
        {
            instantiatePoint = PlayerPrefs.GetInt("SelectedPoint");
        }
        spwanIndex = PlayerPrefs.GetInt("CharaterSelected");
   
        SpwanPlayerinMenu(spwanIndex);
        if (playerobj.name == "Player01(Clone)")
            PlayerPrefs.SetInt(playerobj.name, 1);
    }

    private void SpwanPlayerinMenu(int index)
    {
        playerobj = Instantiate(playersPrefabs[index], instantiatePoints[instantiatePoint].position, instantiatePoints[instantiatePoint].rotation);
        player = playerobj;
        if (camera != null)
            camera.transform.parent = playerobj.transform.GetChild(1);
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            playerobj.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            playerobj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void DestroyLastCharater(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void moveLeft()
    {
        DestroyLastCharater(playerobj);
        spwanIndex--;
        if (spwanIndex < 0)
            spwanIndex = playersPrefabs.Count - 1;

        SpwanPlayerinMenu(spwanIndex);
    }
    public void moveright()
    {
        DestroyLastCharater(playerobj);
        spwanIndex++;
        if(spwanIndex == playersPrefabs.Count)
        {
            spwanIndex = 0;
        }
        SpwanPlayerinMenu(spwanIndex);
    }

    public void ConfirmButton()
    {
        PlayerPrefs.SetInt("CharaterSelected", spwanIndex);
    }

    public void LoadScene(string scnceName)
    {
        fader.FadeTo(scnceName);
    }
    public int GetInstantiatePoint()
    {
        instantiatePoint = PlayerPrefs.GetInt("SelectedPoint");
        return instantiatePoint;
    }

    public void SelectInstantiatePoint(int selectNum)
    {
        PlayerPrefs.SetInt("SelectedPoint", selectNum);
    }
}
