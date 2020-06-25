using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public GameObject tileMapManager;

    public void InstantiateLand()
    {
        Instantiate(tileMapManager);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
