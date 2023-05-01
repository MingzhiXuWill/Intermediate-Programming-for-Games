using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundSets", menuName = "ScriptableObjects/BackgroundSets", order = 1)]
public class BackgroundSets : ScriptableObject
{
    public BackgroundSet[] Sets;

    public string createNameList()
    {
        string list = "";

        for (int count = 0; count < Sets.Length; count++)
        {
            list += Sets[count].name;
            if (count < Sets.Length - 1)
            {
                list += ", ";
            }
        }

        return list;
    }

    public Sprite createSprite(string EnvironmentName)
    {
        for (int count = 0; count < Sets.Length; count++)
        {
            if (Sets[count].name == EnvironmentName)
            {
                return(Sets[count].backgrounds[Random.Range(0, Sets[count].backgrounds.Length - 1)]);
            }
        }

        return null;
    }
}

[System.Serializable]
public struct BackgroundSet
{
    public string name;
    public Sprite[] backgrounds;
}