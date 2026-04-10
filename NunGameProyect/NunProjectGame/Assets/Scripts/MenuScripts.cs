using UnityEngine;
using UnityEngune.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject MainMenu;
    
    public void OpenOptionsMenu()
    {
    	MainMenu.SetActive(false);
    	OptionsMenu.setActive(true);
    }
    
    public void OpenMainMenuPanel()
    {
    	MainMenu.SetActive(true);
    	OptionsMenu(false);
    }
    
    public void QuitGame()
    {
    	Application.Quit();
    }
    
    public void PlayGame()
    {
    	//SceneManager.LoadScene("Level");
    	Console.writeline("Se presionó el boton juga");
    }
    
}
