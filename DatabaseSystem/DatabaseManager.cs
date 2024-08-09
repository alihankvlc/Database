using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace DatabaseSystem
{
    public sealed class DatabaseManager : MonoBehaviour
    {
        [SerializeField] private List<IDatabase> _databases = new();

        private void Awake()
        {
            var loadDatabaseAssets = Resources.LoadAll<ScriptableObject>("");

            loadDatabaseAssets.ForEach(r =>
            {
                if (r is IDatabase database)
                {
                    _databases.Add(database);
                    database.Initialize();
                }
            });
        }
    }
}