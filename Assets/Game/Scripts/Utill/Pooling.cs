using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling<T> where T : Component
{
    private T _BaseObj = null;
    private Transform _Root = null;

    private Queue<T> _PoolingObjs = new Queue<T>();
    private List<T> _UseObjs = new List<T>();

    public int count { get { return _UseObjs.Count; } }

    public Pooling(int poolingCount, T baseObj, Transform root)
    {
        _BaseObj = baseObj;
        _Root = root;

        for (int i = 0; i < poolingCount; i++)
        {
            Create(_Root);
        }

        Delete(baseObj);
    }

    private void Create(Transform root)
    {
        var obj = Object.Instantiate(_BaseObj, root);

        obj.gameObject.SetActive(false);

        _PoolingObjs.Enqueue(obj);
    }

    public void Delete(T obj)
    {
        if(_Root != null)
            obj.transform.SetParent(_Root);

        obj.gameObject.SetActive(false);

        _UseObjs.Remove(obj);
        _PoolingObjs.Enqueue(obj);
    }

    public void DeleteAll()
    {
        for (int i = 0; i < _UseObjs.Count; i++)
        {
            T obj = _UseObjs[i];

            if (_Root != null)
                obj.transform.SetParent(_Root);

            obj.gameObject.SetActive(false);

            _PoolingObjs.Enqueue(obj);
        }

        _UseObjs.Clear();
    }

    public T Get()
    {
        if (_PoolingObjs.Count == 0)
            Create(_Root);

        var obj = _PoolingObjs.Dequeue();

        obj.gameObject.SetActive(true);

        _UseObjs.Add(obj);

        return obj;
    }

    public T Get(Vector3 position)
    {
        if (_PoolingObjs.Count == 0)
            Create(_Root);

        var obj = _PoolingObjs.Dequeue();

        obj.transform.position = position;
        obj.gameObject.SetActive(true);

        _UseObjs.Add(obj);

        return obj;
    }

    public List<T> GetAll()
    {
        return _UseObjs;
    }
}
