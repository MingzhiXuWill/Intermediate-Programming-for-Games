using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act
{
    public string placeName;
    public string placeDescription;
    public string environment;

    public string[] events = new string[10];
    public string[] NPCs = new string[10];

    public void ReceiveInfoFromJson(ActJsonReceiver json)
    {
        placeName = json.place_name;
        placeDescription = json.place_description;
        environment = json.environment;

        events[0] = json.event_01;
        events[1] = json.event_02;
        events[2] = json.event_03;
        events[3] = json.event_04;
        events[4] = json.event_05;
        //events[5] = json.event_06;
        //events[6] = json.event_07;
        //events[7] = json.event_08;
        //events[8] = json.event_09;
        //events[9] = json.event_10;

        NPCs[0] = json.NPC_01;
        NPCs[1] = json.NPC_02;
        NPCs[2] = json.NPC_03;
        NPCs[3] = json.NPC_04;
        NPCs[4] = json.NPC_05;
        //NPCs[5] = json.NPC_06;
        //NPCs[6] = json.NPC_07;
        //NPCs[7] = json.NPC_08;
        //NPCs[8] = json.NPC_09;
        //NPCs[9] = json.NPC_10;
    }
}
