using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Database/Create BuildingData", order = 2)]
public sealed class BuildingData : DatabaseEntry
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    
    public override int ID => _id;
    public override string Name => _name;
}