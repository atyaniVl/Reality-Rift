using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] AudioClip leverOn;
    [SerializeField] AudioClip leverOff;
    [SerializeField] MovingPlatform platform;
    public bool _switch {  get; private set; }
    void Start()
    {
        _switch = false;
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if(collision.transform.position.x > transform.position.x && _switch == true) //right
            {
                GetComponentInChildren<Animator>().SetTrigger("leverOff");
                Debug.Log("leverOff");
                _switch = false;
                SoundManager.instance.PlaySound(leverOff);
                platform.AudioState(false);
            }
            else if(collision.transform.position.x <= transform.position.x && _switch == false)
            {
                GetComponentInChildren<Animator>().SetTrigger("leverOn");//left
                Debug.Log("leverOn");
                _switch = true;
                SoundManager.instance.PlaySound(leverOn);
                platform.AudioState(true);
            }
        }
    }
}
