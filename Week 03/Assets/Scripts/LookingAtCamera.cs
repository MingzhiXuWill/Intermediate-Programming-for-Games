using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Will.Utilities {
    public class LookingAtCamera : MonoBehaviour
    {
        Transform targetToLook;
        void Start()
        {
            targetToLook = Camera.main.transform;
        }

        void Update()
        {
            transform.LookAt(targetToLook);
        }
    }
}