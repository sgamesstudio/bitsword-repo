using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour {
    public BoxCollider2D attackCol;
    public Transform cam;

    public void AttackCol()
    {
        Debug.Log("AttackCol()");
        attackCol.enabled = true;
    }
    public void DestoryAttackCol()
    {
        attackCol.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);
            collision.gameObject.GetComponent<EnemyController>().kill();
            
            //ShakeScreen();
        }
    }

    void ShakeScreen()
    {
        StartCoroutine("shake");
    }

    IEnumerator shake()
    {
        cam.position = new Vector3(cam.position.x + 1f, cam.position.y + 1f, cam.position.z);
        yield return new WaitForSeconds(0.1f);
        cam.position = new Vector3(cam.position.x - 1f, cam.position.y - 1f, cam.position.z);
    }
}
