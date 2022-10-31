using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpell : Spell
{
    private List<int> hitId;
    // Start is called before the first frame update
    void Start()
    {
        hitId = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spellDuration <= 0.0f){
            Destroy(gameObject);
            hitId = null;
        }

        spellDuration -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider col){
        Enemy enemy = col.gameObject.GetComponent<Enemy>();

        if(enemy != null && !hitId.Contains(enemy.GetInstanceID())){
            Debug.Log(enemy.GetInstanceID());
            Debug.Log("Dealing damage: " + enemy.maxHp * 0.8f);
            enemy.GetDamaged(enemy.maxHp * 0.8f);
            hitId.Add(enemy.GetInstanceID());
        }
    }
}
