using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform levelStart;
        [SerializeField] private DS_PlayerController playerControllerSource;

        private PlayerController _playerController;
        private IEnumerator Start()
        {
            while (_playerController == null)
            {
                //TODO: Get reference to player controller from ReferenceManager/DataSource
                _playerController = playerControllerSource.Reference;
                yield return null;
            }
            _playerController.SetPlayerAtLevelStartAndEnable(levelStart.position);
        }
    }
}
