using System;
using UnityEngine;

public static class Models
{
    #region - Player -

    [Serializable]
    public class PlayerSettings
    {
        [Header("Cam Settings")]
        public float sensX;
        public float sensY;
        public bool invertX;
        public bool invertY;

        public float yClampMin = -60f;
        public float yClampMax = 50f;
    }

    #endregion
}
