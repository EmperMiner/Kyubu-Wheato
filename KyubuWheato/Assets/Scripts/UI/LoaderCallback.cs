using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
         if (isFirstUpdate)
         {
            isFirstUpdate = false;
            MainMenu.LoaderCallback();
         }
    }
}
