using UnityEngine;

namespace fwVoiceChat.Scripts
{
    public class PlayersController : MonoBehaviour
    {
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
}