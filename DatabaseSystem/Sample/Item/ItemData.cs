using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Database/Create ItemData", order = 1)]
public sealed class ItemData : DatabaseEntry
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    
    public override int ID => _id;
    public override string Name => _name;
}