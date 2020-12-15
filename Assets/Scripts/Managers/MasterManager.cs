using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private static List<Color> colorList = new List<Color>()
    {
        Color.red,
        Color.green,
        Color.yellow,
        Color.magenta,
        new Color(255F, 0F, 255F),
        new Color(0F, 255F, 255F),
        new Color(255F, 255F, 0F),
        new Color(128F, 0F, 128F)
    };

    public static bool isColorIndexValid(int index){
        if(index > -1 && index < colorList.Count){
            return true;
        }else{
            return false;
        }
    }
    public static Color getColorByIndex(int index)
    {
        if(isColorIndexValid(index)){
            return colorList[index];
        }else{
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
}
