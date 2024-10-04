using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class EditorStartup
{
    static EditorStartup()
    {
        // Attach to the playModeStateChanged event
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode) // Check if we're about to enter Play mode
        {
            // Check if the current active scene is the "Start Screen"
            if (EditorSceneManager.GetActiveScene().name != "Start Screen")
            {
                // Find the "Start Screen" scene asset
                SceneAsset startScreenScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Start Screen.unity");

                if (startScreenScene != null)
                {
                    // Open the "Start Screen" scene
                    EditorSceneManager.OpenScene("Assets/Scenes/Start Screen.unity");
                }
                else
                {
                    Debug.LogWarning("Start Screen scene not found. Please make sure the path is correct.");
                }
            }
        }
    }
}
