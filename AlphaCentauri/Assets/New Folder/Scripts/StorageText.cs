using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorageText : MonoBehaviour
{
    public GameObject popUp;
    public TMP_Text storageText;

    public void Update()
    {
        StoragePrint();
    }
    public void StoragePrint()
    {
        storageText.text = $"{FishWeight.IkanDiStorage}kg/{FishWeight.MaxBeratStorage}kg";
    }
}
