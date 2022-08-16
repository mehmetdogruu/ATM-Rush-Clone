using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { playing,finished}
    public GameState gameState;
    public static GameManager instance;
    public bool finish = true;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameState = GameState.playing;
        }

        
    }

}
