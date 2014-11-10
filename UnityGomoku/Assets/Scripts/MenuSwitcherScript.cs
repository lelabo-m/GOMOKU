using UnityEngine;
using System.Collections;

public class MenuSwitcherScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SetupGameMenu;
	public GameObject HowtoMenu;
	public GameObject StartMenu;
    public GameObject SettingsMenu;
    public GameObject CreditsMenu;

    public void Start()
    {
        NGUITools.SetActive(SetupGameMenu, false);
        NGUITools.SetActive(SettingsMenu, false);
        NGUITools.SetActive(CreditsMenu, false);
		NGUITools.SetActive(HowtoMenu, false);
		NGUITools.SetActive(StartMenu, false);
    }

	public void LaunchVsIa()
	{
		NGUITools.SetActive(StartMenu, false);
		NGUITools.SetActive(MainMenu, true);
	}

	public void LaunchVsHuman()
	{
		Application.LoadLevel(2);
	}

    public void ShowSetupGame()
    {
		NGUITools.SetActive(MainMenu, false);
		NGUITools.SetActive(StartMenu, true);
    }

	public void ShowHowto()
	{
		NGUITools.SetActive(MainMenu, false);
		NGUITools.SetActive(HowtoMenu, true);
	}

    public void ShowSettings()
    {
        NGUITools.SetActive(MainMenu, false);
		NGUITools.SetActive(SettingsMenu, true);
    }

    public void ShowCredits()
    {
        NGUITools.SetActive(MainMenu, false);
		NGUITools.SetActive(CreditsMenu, true);
    }
	public void ShowMenuQuitCredits()
	{
		NGUITools.SetActive(MainMenu, true);
		NGUITools.SetActive(CreditsMenu, false);
	}
	public void ShowMenuQuitHowto()
	{
		NGUITools.SetActive(MainMenu, true);
		NGUITools.SetActive(HowtoMenu, false);
	}
	public void ShowMenuQuitSettings()
	{
		NGUITools.SetActive(MainMenu, true);
		NGUITools.SetActive(SettingsMenu, false);
	}
}
