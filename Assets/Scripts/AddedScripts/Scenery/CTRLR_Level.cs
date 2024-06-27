using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scenery
{
    [Serializable]
    public class CTRLR_Level
    {
        [field: SerializeField] public List<string> SceneNames { get; private set; }
    }
}