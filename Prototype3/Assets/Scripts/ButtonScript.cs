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
        CLOSE
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

            default:
                break;
        }
    }
}
