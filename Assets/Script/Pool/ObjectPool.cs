using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.VersionControl;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Queue<T> pool = new Queue<T>();
    private Transform root;

    private int max_Size;

    private void SetUp(int size)
    {
        for (int i = 0; i < size; i++)
        {
            var obj = Object.Instantiate(prefab, root);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    public ObjectPool(T _prefab, int _size, int _max_Size, Transform _root)
    {
        prefab = _prefab;
        max_Size = _max_Size;
        root = _root;
        SetUp(_size);
    }
    public T Spawn(Vector2 pos)
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        obj = Object.Instantiate(prefab, root);
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        (obj as IPoolable)?.OnSpawn();
        (obj as EnemyBase)?.SetEnemyInPool();
        return obj;
    }
    public void DeSpawn(T obj)
    {
        
        if (pool.Count >= max_Size)
        {
            Object.Destroy(obj.gameObject);
            return;
        }
        obj.transform.SetParent(root, false);
        (obj as IPoolable)?.OnDeSpawn();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
