using UnityEngine;

namespace DataSource
{
    public class PopulateDataSource<T> : MonoBehaviour
    {
        [SerializeField] private SO_DataSource<T> _dataSource;
        [SerializeField] private T _reference;

        [ContextMenu("Run OnEnable")]
        protected void OnEnable()
        {
            if (_reference != null)
                _dataSource.Reference = _reference;
        }

        protected void OnDisable()
        {
            if (_dataSource.Reference.Equals(_reference))
            {
                _dataSource.Reference = default;
            }
        }

        public T Reference
        {
            get => _reference;
            set
            {
                _reference = value;
                if (_dataSource != null)
                {
                    _dataSource.Reference = _reference;
                }
            }

        }
    }
}

