using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseButtonScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public bool pauseMenuState;
    private void Start()
    {
        pauseMenuState = false;
        pauseMenu.SetActive(pauseMenuState);
    }
    public void ChangeVisibility()
    {
        pauseMenuState = !pauseMenuState;
        pauseMenu.SetActive(pauseMenuState);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
