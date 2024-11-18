using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPopUp : MonoBehaviour
{
    public GameObject popUp;
    public void Click()
    {
        popUp.SetActive(true);
    }

}
