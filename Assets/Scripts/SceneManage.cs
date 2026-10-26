using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
