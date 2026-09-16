using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine;
using System.Threading.Tasks;

public class GameSceneManager : NetworkBehaviour
{
    private static GameSceneManager instance;

    public static GameSceneManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        LoadGameScene();
    }

    public async Task LoadGameScene()
    {
        await SceneManager.UnloadSceneAsync(Bootstrap.Instance.mainMenuSceneName);
        Debug.Log("Unloaded Menu Scene");
        await SceneManager.LoadSceneAsync(Bootstrap.Instance.gameSceneName, LoadSceneMode.Additive);
        Debug.Log("Loaded Game Scene");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Bootstrap.Instance.gameSceneName));
    }
}
