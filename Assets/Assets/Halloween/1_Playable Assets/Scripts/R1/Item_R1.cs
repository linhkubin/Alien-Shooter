using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_R1 : MonoBehaviour
{
    public Rigidbody2D rb;

    private Vector2 playerDir;
    private float timeStamp;
    private bool moveToPlayer;

    private Player_R1 fighter;

    void Start()
    {
        fighter = Player_R1.Ins;
    }

    void Update()
    {
        if (moveToPlayer)
		{
            playerDir = -(transform.position - fighter.transform.position).normalized;
            rb.velocity = new Vector2(playerDir.x, playerDir.y) * 10f * (Time.time / timeStamp);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name.Equals("Magnet"))
		{
            timeStamp = Time.time;
            moveToPlayer = true;
		}
	}
}
