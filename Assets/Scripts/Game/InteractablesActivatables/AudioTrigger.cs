using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioLibrary.PlayAlarmShort();
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
