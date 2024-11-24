using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HideButton : MonoBehaviour
{
    [SerializeField] Transform reference, centralEventLog;
    Transform player;
    bool debounce = false;
    Button button;
    Hide hide;
    void Start()
    {
        player = reference.GetComponent<ReferenceScript>().player;
        hide = player.GetComponent<Hide>();
        button = transform.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            print(hide.cooldownLeft);
            if (!hide.isHide && hide.cooldownLeft <= 0)
            {
                AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.BushShake]);
                hide.StartHide();
            }
            else if (!hide.isHide && hide.cooldownLeft > 0)
            {
                centralEventLog.GetComponent<EventLog>().Log("Cooldown still active", 1.5f);
            }
            else
            {
                AudioManager.Instance.PlaySFX(AudioRegistry.Sounds[AudioManager.Sound.BushShake]);
                hide.StopHide();
                debounce = false;
            }
        });

    }
    void Update()
    {

    }
}
