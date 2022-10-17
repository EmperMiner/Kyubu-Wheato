using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLoaderCallback : MonoBehaviour
{
    private bool isIngameFirstUpdate = true;

    private void Update()
    {
         if (isIngameFirstUpdate)
         {
            isIngameFirstUpdate = false;
            GameOverScreen.IngameLoaderCallback();
         }
    }
}