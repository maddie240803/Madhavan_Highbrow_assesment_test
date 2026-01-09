using UnityEngine;
using UnityEngine.SceneManagement;



public class ReplayGame : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // We explicitly use UnityEngine.Debug to prevent conflicts
        UnityEngine.Debug.Log("Quit Game Requested");

#if UNITY_EDITOR
           
#else
        // This line only exists in the built game
        UnityEngine.Application.Quit();
#endif
    }
}