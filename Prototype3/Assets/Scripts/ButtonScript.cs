using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public enum Type
    {
        PLAY,
        RETURN,
        CLOSE,
        HELP,
        HELPOFF
    }
    public Type buttonType;

    private void OnMouseDown()
    {
        switch (buttonType)
        {
            case Type.PLAY:
                {
                    // Load game
                    SceneManager.LoadScene("Level Design 02");

                    break;
                }
            case Type.CLOSE:
                {
                    // Close program
                    Application.Quit();

                    break;
                }
            case Type.RETURN:
                {
                    // Load menu
                    SceneManager.LoadScene("Menu");

                    break;
                }
            case Type.HELP:
                {
                    Camera.main.transform.position = new Vector3(-39.2f, 1.96f, -20.87f);

                    break;
                }
            case Type.HELPOFF:
                {
                    Camera.main.transform.position = new Vector3(0.0f, 1.0f, -15.9f);

                    break;
                }

            default:
                break;
        }
    }
}
