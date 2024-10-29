using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETest : MonoBehaviour
{
    [SerializeField]
    SoundEffectManager soundEffectManager;

    [SerializeField]
    BackGroundMusicManager backGroundMusicManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.TestSE);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            backGroundMusicManager.OnPlay(BackGroundMusicManager.MusicName.TestBGM);
        }
    }
}
