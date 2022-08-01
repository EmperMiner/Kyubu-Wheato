using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameLoaderCallback : MonoBehaviour
{
    private bool isIngameFirstUpdate = true;

    private void Update()
    {
         if (isIngameFirstUpdate)
         {
            isIngameFirstUpdate = false;
            PauseMenu.IngameLoaderCallback();
         }
    }
}