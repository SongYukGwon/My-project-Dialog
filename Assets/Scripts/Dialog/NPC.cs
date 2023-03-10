using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private GameObject pressF;
    [SerializeField]
    public int id;
    [SerializeField]
    public bool isNPC;
    [SerializeField]
    public string NPCName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("μΈμμλ");
        if (other.gameObject.tag == "Player")
        {
            pressF.SetActive(true);
        };
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            pressF.SetActive(false);
        };
    }

}
