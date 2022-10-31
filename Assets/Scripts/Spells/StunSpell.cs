using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSpell : Spell
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spellDuration <= 0.0f){
            Destroy(gameObject);
        }

        spellDuration -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider col){
        EnemyStatus status = col.gameObject.GetComponent<EnemyStatus>();
        if(status != null){
            status.ApplyNewStatus("ShockEffect", 0.0f, 0.0f, 5.0f, 0.0f, 1.0f/5.0f, 1.0f);
            status.gameObject.transform.Find("ShockEffect").gameObject.SetActive(true);
            status.gameObject.transform.Find("ShockEffect").GetComponent<ParticleSystem>().Play();
        }
    }
}
