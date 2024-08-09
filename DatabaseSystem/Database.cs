using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public abstract class DatabaseEntry : ScriptableObject
{
    public abstract int ID { get; }
    public abstract string Name { get; }
}

public interface IDatabase
{
    void Initialize();
    void Populate();
    void Clear();
    Type GetDataType { get; }
}

public abstract class Database<T1, T2> : ScriptableObject, IDatabase
    where T1 : DatabaseEntry where T2 : Database<T1, T2>
{
    private static T2 _instance;

    public static T2 Instance
    {
        get
        {
            if (_instance == null)
            {
                var type = typeof(T2);
                var databaseName = type.Name;
                var path = $"Assets/Resources/{databaseName}.asset";

                _instance = AssetDatabase.LoadAssetAtPath<T2>(path);

                if (_instance == null)
                    Debug.Log($"{path} not found. Creating new one.");
            }

            return _instance;
        }
    }

    [SerializeField, InlineEditor, Searchable]
    private List<T1> _entries;

    public IReadOnlyList<T1> DatasEntries => _entries;
    public Type GetDataType => typeof(T1);

    private Dictionary<int, T1> _cache = new();

    public void Initialize()
    {
        _cache = _entries.ToDictionary(r => r.ID);
    }

    public void AddData(T1 entry)
    {
        if (_cache.ContainsKey(entry.ID))
        {
            Debug.LogError($"Entry with ID {entry.ID} already exists.");
            return;
        }

        _entries.Add(entry);
    }

    public void RemoveData(T1 entry)
    {
        if (!_cache.ContainsKey(entry.ID))
        {
            Debug.LogError($"Entry with ID {entry.ID} does not exist.");
            return;
        }

        _entries.Remove(entry);
        _cache.Remove(entry.ID);
    }

    public bool HasData(int id)
    {
        return _entries.Any(r => r.ID == id);
    }

    public T1 GetData(int id)
    {
        if (_cache.TryGetValue(id, out T1 data))
            return data;

        Debug.LogError($"Entry with ID {id} does not exist.");
        return null;
    }

    [ButtonGroup("Database")]
    [Button("Populate Data")]
    public void Populate()
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T1)}");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(T1));

            AddData(obj as T1);
        }
    }

    [ButtonGroup("Database")]
    [Button("Clear All Data")]
    public void Clear()
    {
        _entries.Clear();
        _cache.Clear();
    }
}