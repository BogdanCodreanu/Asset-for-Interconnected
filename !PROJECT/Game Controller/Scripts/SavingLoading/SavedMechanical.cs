using System.Collections.Generic;
[System.Serializable]
public class SavedMechanical {
    public SavedMechPart[] savedParts;

    public int ssWidth;
    public int ssHeight;
    public byte[] screenshot;

    public SavedMechanical(int Length) {
        savedParts = new SavedMechPart[Length];
    }
}

[System.Serializable]
public class SavedMechPart {
    public SavedMechPart(int indexDatabase, int spawnIndex) {
        mechIndexFromDatabase = indexDatabase;
        indexOfSpawn = spawnIndex;
    }
    public int mechIndexFromDatabase;
    public int indexOfSpawn;
    public string mechData;
}
