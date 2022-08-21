using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReceiverDragon : MonoBehaviour
{

    [SerializeField] private AudioSource attack1VO;
    [SerializeField] private AudioSource footStep;
    [SerializeField] private AudioSource deathFall;
    [SerializeField] private AudioSource deathVO;
    [SerializeField] private AudioSource deathSweetener;
    [SerializeField] private AudioSource idleLeather;
    [SerializeField] private AudioSource idleVO;
    [SerializeField] private AudioSource exhale;
    [SerializeField] private AudioSource runExhale;
    [SerializeField] private AudioSource runningStart;

    void PlayAudio(string audio)
    {
        switch (audio)
        {
            case "Attack1VO":
                attack1VO.Play();
                break;
            case "DeathFall":
                deathFall.Play();
                break;
            case "DeathVO":
                deathVO.Play();
                break;
            case "DeathSweetener":
                deathSweetener.Play();
                break;
            case "IdleLeather":
                idleLeather.Play();
                break;
            case "IdleVO":
                idleVO.Play();
                break;
            case "Footstep":
                footStep.Play();
                break;
            case "Exhale":
                exhale.Play();
                break;
            case "RunExhale":
                runExhale.Play();
                break;   
            case "RunningStart":
                runningStart.Play();
                break;
            default:
                Debug.LogError("Unrecognized song : " + audio);
                break;
        }
    }
}
