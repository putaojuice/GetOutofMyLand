using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    [SerializeField] private Spell spell;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell(){
        Instantiate(spell, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
