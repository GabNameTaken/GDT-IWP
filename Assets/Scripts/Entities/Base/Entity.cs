using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base/Entity")]
public class Entity : ScriptableObject
{
    [SerializeField] string _entityName;
    public string entityName => _entityName;

    [SerializeField] ClassType _classType;
    public ClassType classType => _classType;

    [SerializeField] Element _element;
    public Element element => _element;

    [SerializeField] Stats _baseStats;
    public Stats baseStats => _baseStats;

    [SerializeField] SkillSet _baseSkillSet;
    public SkillSet baseSkillSet =>_baseSkillSet;
}
