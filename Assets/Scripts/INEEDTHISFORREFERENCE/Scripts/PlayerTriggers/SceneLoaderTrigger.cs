using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTrigger : MonoBehaviour
{
    [SerializeField] private int sceneIndexToLoad;

    public void LoadScene()
    {
        InputManager.Instance.PlayerInputActions.Player1.Disable();
        SceneManager.LoadScene(sceneIndexToLoad);
    }
}