using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillBall", menuName = "LumiProject/SkillBall")]
public class SkillBall : MonoBehaviour
{
    public string BallName;
    public float BallSpeed;
    public float BallRotageSpeed;
    public float BallDamage;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ememi"))
        //    Destroy(gameObject);

        if(collision.gameObject.CompareTag("Localwall") || collision.gameObject.CompareTag("ground"))
            Destroy(gameObject);
    }


}
