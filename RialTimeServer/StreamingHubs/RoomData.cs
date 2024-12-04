using Shared.Interfaces.StreamingHubs;
using UnityEngine;

namespace RialTimeServer.StreamingHubs
{
    public class RoomData
    {
        public JoinedUser JoinedUser { get; set; }

        public Vector3 pos {  get; set; }

        public Quaternion rot { get; set; }
    }
}
