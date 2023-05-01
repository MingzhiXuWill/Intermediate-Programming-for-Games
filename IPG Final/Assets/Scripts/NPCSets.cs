using UnityEngine;

[CreateAssetMenu(fileName = "NPCSets", menuName = "ScriptableObjects/NPCSets", order = 1)]
public class NPCSets : ScriptableObject
{
    public NPCSet[] Sets;

    public string createNameList()
    {
        string list = "";

        for (int count = 0; count < Sets.Length; count ++)
        {
            list += Sets[count].name;
            if (count < Sets.Length - 1)
            {
                list += ", ";
            }
        }

        return list;
    }

    public Sprite createSprite(string NPCName)
    {
        for (int count = 0; count < Sets.Length; count++)
        {
            if (Sets[count].name == NPCName)
            {
                return (Sets[count].npcs[Random.Range(0, Sets[count].npcs.Length - 1)]);
            }
        }

        return null;
    }
}

[System.Serializable]
public struct NPCSet
{
    public string name;
    public Sprite[] npcs;
}
