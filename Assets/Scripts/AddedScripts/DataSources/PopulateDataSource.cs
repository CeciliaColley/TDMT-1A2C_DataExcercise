using UnityEngine;
using DataSource;

public class PopulateDataSource<T> : MonoBehaviour
{
    [SerializeField] private SO_DataSource<T> dataSource;
    [SerializeField] private T reference;

    [ContextMenu("Run OnEnable")]
    protected void OnEnable()
    {
        if (reference != null)
            dataSource.Reference = reference;
    }

    protected void OnDisable()
    {
        if (dataSource.Reference.Equals(reference))
        {
            dataSource.Reference = default;
        }
    }
}

