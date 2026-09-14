using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private static Bootstrap instance;
    public static Bootstrap Instance{
        get
        {
            return instance;
        }
        private set
        {
            
        }
    }

    [Header("Critical Scenes")]
    public string mainMenuSceneName;
    [SerializeField] private string bootstrapSceneName;
    public string gameSceneName;

    private void Awake()
    {
        Debug.Log("initializing bootstrap!");
        instance = this;
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Additive);
    }

}
