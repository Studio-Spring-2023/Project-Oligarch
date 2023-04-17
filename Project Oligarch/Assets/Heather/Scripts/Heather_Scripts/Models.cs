using System;
using UnityEngine;

public static class Models
{
    #region - Player -

    [Serializable]
    public class CamSettings
    {
        [Header ( "Cam Settings" )]
        public float sensX;
        public float sensY;
        public bool invertX;
        public bool invertY;

        public float yClampMin = -60f;
        public float yClampMax = 50f;

        [Header ( "Player" )]
        public float playerRotateSmooth = 1f;
    }

    [Serializable]
    public class PlayerControl
    {
        public float forwardSpeed = 1f;
        public float playerSmooth = 0.6f;
    }

    #endregion
}
