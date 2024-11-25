using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePointAttacher : MonoBehaviour
{
    [SerializeField] List<Transform> points;
    [SerializeField] List<Transform> rods;
    Transform rodAttachObect;
    int index = 0;
    void Start()
    {
        rodAttachObect = GameObject.Find("RodAttach").transform;
    }
    void Update()
    {
        rodAttachObect.position = points[index].position;
    }
    public void Equip(int index)
    {
        rods[this.index].gameObject.SetActive(false);
        Debug.Log("helo");
        rods[this.index].GetComponent<Animator>().SetBool("unequipped", true);

        rods[index].gameObject.SetActive(true);
        rods[index].GetComponent<Animator>().SetBool("unequiped", false);
    }
    public void Cast()
    {
        rods[index].GetComponent<Animator>().SetTrigger("cast");
    }
    public void Reel(bool isReeling)
    {
        rods[index].GetComponent<Animator>().SetBool("reeling", isReeling);
    }
    public void Unequip()
    {
        rods[index].GetComponent<Animator>().SetBool("unequiped", true);
    }

}
