using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator doorAnimator; // Animator for the door animation

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        doorAnimator.SetBool("Close", false); // Assumes the door animator has an "Open" trigger
        Destroy(gameObject);
    }
}
