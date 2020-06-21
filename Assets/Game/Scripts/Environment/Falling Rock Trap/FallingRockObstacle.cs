﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///-------------------------------------------------------------------------------------------------
// file: FallingRockObstacle.cs
//
// author: Rishi Barnwal
// date: 06/18/2020
//
// summary: Overrides of the IObstacle abstract class to reset the falling rock trap
///-------------------------------------------------------------------------------------------------
///

public class FallingRockObstacle : IObstacle
{
    public GameObject rock;
    public GameObject trigger;

    public Vector3 initialRockPosition;

    //public override void ResetObstacle()
    //{
    //    trigger.SetActive(true);
    //    rock.transform.localPosition = initialRockPosition;
    //    rock.GetComponent<Rigidbody>().useGravity = false;
    //    isTriggered = false;
    //}

    public override void UnresetObstacle()
    {
        trigger.SetActive(false);
        rock.GetComponent<Rigidbody>().useGravity = true;

        Physics.Raycast(rock.transform.position, Vector3.down, out RaycastHit hit);

        rock.transform.position = hit.point;
        isTriggered = true;
    }
}
