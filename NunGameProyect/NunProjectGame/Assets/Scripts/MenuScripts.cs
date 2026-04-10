using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject MainMenu;
    
    public void OpenOptionsPanel()
    {
    	MainMenu.SetActive(false);
    	OptionsMenu.SetActive(true);
    }
    
    public void OpenMainMenuPanel()
    {
    	MainMenu.SetActive(true);
    	OptionsMenu.SetActive(false);
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
