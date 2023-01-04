using UnityEngine;

public interface IF_GameCharacter
{
    //Vector3Int TileVector { get; set; }

    Vector2Int TileVector { get; set; }

    float Health { get; set; }

    void GetDamage(float i_dmgVal);
}
