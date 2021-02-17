using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetColour : MonoBehaviour
{

public Material green;
public Material red;

public GameObject GoalTargettopright;
public GameObject GoalTargettopleft;

    private void OnTriggerEnter(Collider other)
     {
        if(other.gameObject.name == "Football")
        {
            GetComponent<Renderer>().material = green;
        }
     }
 
    public void ResetColour()
   {
      GoalTargettopright.GetComponent<Renderer>().material = red;
      GoalTargettopleft.GetComponent<Renderer>().material = red;
   }

}

