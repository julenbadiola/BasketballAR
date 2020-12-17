using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

using System.Linq;

public class FinalScoreScene : MonoBehaviour
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private FinalScoreListing _finalScoreListing;

    public void SetFinalResults(Dictionary<string, int[]> data)
    {
        foreach (var item in data)
        {
            Debug.Log("FINAL RESULT: " + item.Key + " - " + string.Join(", ", item.Value.Select(i => i.ToString()).ToArray()));
            AddFinalScoreListing(item.Key, item.Value);
        }
    }
    public void AddFinalScoreListing(string nickname, int[] dataOfPlayer)
    {
        FinalScoreListing listing = Instantiate(_finalScoreListing, _content);
        int color = dataOfPlayer[0];
        int throws = dataOfPlayer[1];
        int score = dataOfPlayer[2];

        if (listing != null)
        {
            listing.SetData(nickname, color, throws, score);
        }
    }
}