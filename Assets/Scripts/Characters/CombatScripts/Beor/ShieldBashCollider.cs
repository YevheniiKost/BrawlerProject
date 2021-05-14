using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldBashCollider : MonoBehaviour
{

    private List<CharacterIdentifier> contacts = new List<CharacterIdentifier>();

    public List<CharacterIdentifier> GetContacts()
    {
        return contacts;
    }

    public void ClearContacts()
    {
        contacts.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterIdentifier character))
        {
            if (contacts.Contains(character))
            {
                return;
            }
            else
            {
                contacts.Add(character);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
