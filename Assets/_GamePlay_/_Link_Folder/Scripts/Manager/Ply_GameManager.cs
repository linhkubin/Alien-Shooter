using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay}

public class Ply_GameManager : Ply_Singleton<Ply_GameManager>
{
    public Ply_Ship ship;

    public static GameState gameState = GameState.MainMenu;

}
