using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act
{
    public string placeName;
    public string placeDescription;

    public string[] events = new string[10];

    public void ReceiveInfoFromJson(ActJsonReceiver json)
    {
        placeName = json.place_name;
        placeDescription = json.place_description;

        events[0] = json.event_01;
        events[1] = json.event_02;
        events[2] = json.event_03;
        events[3] = json.event_04;
        events[4] = json.event_05;
        events[5] = json.event_06;
        events[6] = json.event_07;
        events[7] = json.event_08;
        events[8] = json.event_09;
        events[9] = json.event_10;
    }
}
