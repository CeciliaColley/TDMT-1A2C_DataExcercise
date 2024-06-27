using UnityEngine;
using DataSource;

namespace Scenery
{
    [CreateAssetMenu(menuName = "Data/Sources/Scenery Manager", fileName = "Source_SceneryData", order = 0)]
    public class SO_Scenery : SO_DataSource<MGR_Scenery> { }
}