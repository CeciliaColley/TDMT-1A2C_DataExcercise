using UnityEngine;

namespace DataSource
{
    /// <summary>
    /// An abstract class for ScriptableObject data sources.
    /// </summary>
    /// <typeparam name="T">The type of the data source.</typeparam>
    public abstract class SO_DataSource<T> : ScriptableObject
    {
        /// <summary>
        /// The reference value of the data source.
        /// </summary>
        [SerializeField] private T _reference;

        /// <summary>
        /// Indicates whether logging is enabled.
        /// </summary>
        [SerializeField] private bool logEnabled = true;

        /// <summary>
        /// Gets or sets the reference value.
        /// Logs the change if logging is enabled and the new value is not null.
        /// </summary>
        public T Reference
        {
            get => _reference;
            set
            {
                if (value != null && logEnabled)
                    Debug.Log($"{name}: Changed value to {value}");
                _reference = value;
            }
        }
    }
}


