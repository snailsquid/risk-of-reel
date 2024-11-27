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
        rods[this.index].GetComponent<Animator>().SetBool("unequipped", true);

        this.index = index;
        rods[index].gameObject.SetActive(true);
        rods[index].GetComponent<Animator>().SetBool("unequiped", false);
    }
    public void Cast()
    {
        rods[index].GetComponent<Animator>().SetTrigger("cast");
        AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.RodCast]);
    }
    public void Reel(bool isReeling)
    {
        rods[index].GetComponent<Animator>().SetBool("reeling", isReeling);
    }
    public void Unequip()
    {
        rods[index].GetComponent<Animator>().SetBool("unequiped", true);
        AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.RodCast]);
    }

}
