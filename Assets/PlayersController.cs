using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectRandomRoleForAll(int traitors_count = 2)
    {
        foreach(PlayerStats ps in FindObjectsOfType<PlayerStats>())
		{
            int role = Random.Range(0, 2);
            if(traitors_count == 0) role = 0;
            if ((Role)role == Role.Traitor) traitors_count--;

            ps.PlayerRole = (Role)role;
        }
    }
}