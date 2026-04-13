using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject MainMenu;
    public GameObject BackToGame;
    public GameObject BackToMainMenu;
    public void OpenOptionsPanel()
    {
    	MainMenu.SetActive(false);
    	OptionsMenu.SetActive(true);
    }
    public void ResumeGame()
    {
         Debug.Log("En el juego");
    }
    public void OpenMainMenuPanel()
    {
    	MainMenu.SetActive(true);
    	OptionsMenu.SetActive(false);
    }
    public void OpenMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Back to menu");
    }
    
    public void QuitGame()
    {
    	Application.Quit();
    }
    
    public void PlayGame()
    {
    	//SceneManager.LoadScene("Level 1");
        Debug.Log("Play Game");
    	
    }
    
}
