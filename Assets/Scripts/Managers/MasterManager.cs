using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private GameSettings _gameSettings;
    public static GameSettings GameSettings {
        get {
            return Instance._gameSettings;
        }
    }

    //Online events
    public static byte SCORE_UPDATE = 0;
    public static byte SCORE_NORMALIZATION = 1;
    public static byte SCORE_REACHED = 2;
    public static byte PLAYER_INSTANTIATION = 3;

    private static List<Color> colorList = new List<Color>()
    {
        Color.red,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };

    public static Color GetColorOfPlayer(Player player)
    {
        if(player.CustomProperties.ContainsKey("Color")){
            int colorIndex = (int) PhotonNetwork.LocalPlayer.CustomProperties["Color"];
            return MasterManager.getColorByIndex(colorIndex);
        }
        else
        {
            return Color.white;
        }
    }

    public static bool isColorIndexValid(int index)
    {
        if(index > -1 && index < colorList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static Color getColorByIndex(int index)
    {
        if(isColorIndexValid(index))
        {
            return colorList[index];
        }
        else
        {
            return Color.white;
        }
        
    }

    public static Color getRandomColor()
    {
        int i = getRandomColorIndex();
        return getColorByIndex(i);
    }

    public static int getRandomColorIndex()
    {
        return Random.Range(0, colorList.Count);
    }

    public static int getNextColorIndex(int index)
    {
        if( index == colorList.Count - 1 )
        {
            return 0;
        }
        else
        {
            return index + 1;
        }
    }

    public static void SetFinalScoreScene(Dictionary<string, Vector2> data)
    {
        SceneManager.LoadScene(2);
        Debug.Log("Sigue haciendo!!!");
    }
}
