using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	public SceneFader fader;

	public GameObject caveButton;
	public GameObject dungeonButton;
	public GameObject forestDayButton;
	public GameObject forestNightButton;
	public GameObject desertButton;

	public Button[] caveLevelButtons;
	public Button[] dungeonLevelButtons;
	public Button[] forestDayLevelButtons;
	public Button[] forestNightLevelButtons;
	public Button[] desertLevelButtons;

	[HideInInspector]
	public int caveLevelReached;
	[HideInInspector]
	public int dungeonLevelReached;
	[HideInInspector]
	public int forestDayLevelReached;
	[HideInInspector]
	public int forestNightLevelReached;
	[HideInInspector]
	public int desertLevelReached;

	#region Singleton 
	public static LevelSelector instance;
	void Awake()
	{
		instance = this;
	}
    #endregion

    private void Start()
	{
		caveLevelReached = PlayerPrefs.GetInt("CavelevelReached", 1);
		dungeonLevelReached = PlayerPrefs.GetInt("DungeonlevelReached", 1);
		forestDayLevelReached = PlayerPrefs.GetInt("ForestDaylevelReached", 1);
		forestNightLevelReached = PlayerPrefs.GetInt("ForestNightlevelReached", 1);
		desertLevelReached = PlayerPrefs.GetInt("DeserttlevelReached", 1);
	}
	void Update ()
	{	
		if (caveButton.activeInHierarchy)
		{
			caveLevelReached = PlayerPrefs.GetInt("CavelevelReached", 1);
			for (int i = 0; i < caveLevelButtons.Length; i++)
			{
				if (i + 1 > caveLevelReached)
					caveLevelButtons[i].interactable = false;
			}
		}
		if (dungeonButton.activeInHierarchy)
		{
			dungeonLevelReached = PlayerPrefs.GetInt("DungeonlevelReached", 1);
			for (int i = 0; i < dungeonLevelButtons.Length; i++)
			{
				if (i + 1 > dungeonLevelReached)
					dungeonLevelButtons[i].interactable = false;
			}
		}
		if (forestDayButton.activeInHierarchy)
		{
			forestDayLevelReached = PlayerPrefs.GetInt("ForestDaylevelReached", 1);
			for (int i = 0; i < forestDayLevelButtons.Length; i++)
			{
				if (i + 1 > forestDayLevelReached)
					forestDayLevelButtons[i].interactable = false;
			}
		}
		if (forestNightButton.activeInHierarchy)
		{
			forestNightLevelReached = PlayerPrefs.GetInt("ForestNightlevelReached", 1);
			for (int i = 0; i < forestNightLevelButtons.Length; i++)
			{
				if (i + 1 > forestNightLevelReached)
					forestNightLevelButtons[i].interactable = false;
			}
		}
		if (desertButton.activeInHierarchy)
		{
			desertLevelReached = PlayerPrefs.GetInt("DeserttlevelReached", 1);
			for (int i = 0; i < desertLevelButtons.Length; i++)
			{
				if (i + 1 > desertLevelReached)
					desertLevelButtons[i].interactable = false;
			}
		}
	}

	public void Select (string levelName)
	{
		fader.FadeTo(levelName);
	}

}
