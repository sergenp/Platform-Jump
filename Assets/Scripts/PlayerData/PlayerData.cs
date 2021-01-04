[System.Serializable]
public class PlayerUpgrades
{
    public int minJumpSpeed = 20;
    public int fullyChargedJumpSpeed = 55;
    public float jumpForceGainMultiplier = 1f;
    public float arrowRotationSpeed = 1.25f;
    public int jumpsInAir = 5;
}

[System.Serializable]
public class PlayerData 
{
    public int currentLevel;
    public int goldAmount;
    
    public PlayerUpgrades upgrades;
}