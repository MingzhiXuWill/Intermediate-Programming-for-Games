using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSets", menuName = "ScriptableObjects/PlayerSets", order = 1)]
public class PlayerSets : ScriptableObject
{
    public PlayerSet[] Sets;

    public Sprite createSprite(int playerNumber)
    {
        return Sets[playerNumber].sprite;
    }

    public Sprite createIcon(int playerNumber)
    {
        return Sets[playerNumber].icon;
    }
}

[System.Serializable]
public struct PlayerSet
{
    public Sprite icon;
    public Sprite sprite;
}
