using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType {
    Pistol, ShotGun, Rifts, Sniper, AUG, DualBerretas, Knife
}
public abstract class Gun {
    public virtual GunType GunType { get { return GunType.Pistol; } }
    public virtual float BulletDamage { get { return 1f; } }
    public virtual float BulletCriticalDamage { get { return 1f; } }
    public virtual int NumberOfRay { get { return 1; } }
    public virtual float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -0,+0,
            };
        }
    }
    // Basic Light
    public virtual float PointLightIntensity { get { return .5f; } }
    public virtual float PointLightRange { get { return .5f; } }
    public virtual Color GunLightColor { get { return Color.red; } }
    public virtual float LrStarWidth { get { return .5f; } }
    public virtual float LrEndWidth { get { return .5f; } }
    // Laser Effect
    public virtual float ShootRange { get { return .5f; } }

    // Shoot Effect
    public virtual float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +4.7f, -4.7f, +3.5f, -3.5f, +2.3f, -2.3f }; ;
            return floatArray;
        }
    }
    public virtual float ShakeIntensity { get { return .5f; } }
    public virtual float IntervalTime { get { return .5f; } }
    public virtual float ForceOpposite { get { return .5f; } }
    public virtual float FireRate { get { return .5f; } }
    public virtual float SlowdownFactor { get { return .5f; } }
    public virtual float FlashLightFactor { get { return 10f; } }
}
public class Rifts : Gun {
    public override float BulletDamage { get { return 4.5f; } }
    public override float BulletCriticalDamage { get { return 9f; } }
    public override GunType GunType { get { return GunType.Rifts; } }
    public override int NumberOfRay { get { return 1; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -0,+0,
            };
        }
    }
    // Basic Rifts Light
    public override Color GunLightColor {
        get {
            return Color.yellow;
        }
    }
    public override float PointLightIntensity { get { return .75f; } }
    public override float PointLightRange { get { return 6.5f; } }
    public override float LrStarWidth { get { return .1f; } }
    public override float LrEndWidth { get { return .05f; } }
    // Laser Rifts Effect
    public override float ShootRange { get { return 8f; } }
    // Shoot Rifts Effect
    public override float FireRate { get { return .15f; } }
    public override float SlowdownFactor { get { return 1f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +9.7f, -9.7f, +8.5f, -8.5f, +7.3f, -7.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return .3f; } }
    public override float IntervalTime { get { return .1f; } }
    public override float ForceOpposite { get { return 10f; } }
    public override float FlashLightFactor { get { return 5f; } }
}
public class ShotGun : Gun {
    public override float BulletDamage { get { return 15f; } }
    public override float BulletCriticalDamage { get { return 21f; } }
    public override GunType GunType { get { return GunType.ShotGun; } }
    public override int NumberOfRay { get { return 3; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0f, -18f,+18f
            };
        }
    }
    // Basic ShotGun Light
    public override Color GunLightColor { get { return Color.green; } }
    public override float PointLightIntensity { get { return 1.5f; } }
    public override float PointLightRange { get { return 9.7f; } }
    public override float LrStarWidth { get { return .05f; } }
    public override float LrEndWidth { get { return .1f; } }
    // Laser ShotGun Effect
    public override float ShootRange { get { return 3.5f; } }
    // Shoot ShotGun Effect
    public override float FireRate { get { return 1f; } }
    public override float SlowdownFactor { get { return .7f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +19.7f, -19.7f, +18.5f, -18.5f, +17.3f, -17.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return .8f; } }
    public override float IntervalTime { get { return .2f; } }
    public override float ForceOpposite { get { return 15f; } }
    public override float FlashLightFactor { get { return 9f; } }
}
public class Pistol : Gun {
    public override float BulletDamage { get { return 7f; } }
    public override float BulletCriticalDamage { get { return 15f; } }
    public override GunType GunType { get { return GunType.Pistol; } }
    public override int NumberOfRay { get { return 1; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -0,+0,
            };
        }
    }
    // Basic Pistol Light
    public override Color GunLightColor { get { return Color.red; } }
    public override float PointLightIntensity { get { return 1f; } }
    public override float PointLightRange { get { return 6.5f; } }
    public override float LrStarWidth { get { return .05f; } }
    public override float LrEndWidth { get { return .05f; } }
    // Laser Pistol Effect
    public override float ShootRange { get { return 7f; } }
    // Shoot Pistol Effect
    public override float FireRate { get { return .5f; } }
    public override float SlowdownFactor { get { return 1f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +14.7f, -14.7f, +13.5f, -13.5f, +12.3f, -12.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return .4f; } }
    public override float IntervalTime { get { return .2f; } }
    public override float ForceOpposite { get { return 12f; } }
    public override float FlashLightFactor { get { return 7.5f; } }
}
public class Sniper : Gun {
    public override float BulletDamage { get { return 43f; } }
    public override float BulletCriticalDamage { get { return 100f; } }
    public override GunType GunType { get { return GunType.Sniper; } }
    public override int NumberOfRay { get { return 1; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -0,+0,
            };
        }
    }
    // Basic Sniper Light
    public override Color GunLightColor { get { return Color.cyan; } }
    public override float PointLightIntensity { get { return 1.5f; } }
    public override float PointLightRange { get { return 9f; } }
    public override float LrStarWidth { get { return .05f; } }
    public override float LrEndWidth { get { return .05f; } }
    // Laser Sniper Effect
    public override float ShootRange { get { return 11f; } }

    // Shoot Sniper Effect
    public override float FireRate { get { return 1.2f; } }
    public override float SlowdownFactor { get { return .8f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +19.7f, -19.7f, +18.5f, -18.5f, +17.3f, -17.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return 1f; } }
    public override float IntervalTime { get { return .3f; } }
    public override float ForceOpposite { get { return 12f; } }
    public override float FlashLightFactor { get { return 7f; } }
}
public class AUG : Gun {
    public override float BulletDamage { get { return 4f; } }
    public override float BulletCriticalDamage { get { return 7f; } }
    public override GunType GunType { get { return GunType.AUG; } }
    public override int NumberOfRay { get { return 3; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -30, +30
            };
        }
    }
    // Basic AUG Light
    public override Color GunLightColor { get { return Color.magenta; } }
    public override float PointLightIntensity { get { return 1.5f; } }
    public override float PointLightRange { get { return 9f; } }
    public override float LrStarWidth { get { return .05f; } }
    public override float LrEndWidth { get { return .05f; } }
    // Laser AUG Effect
    public override float ShootRange { get { return 7f; } }

    // Shoot AUG Effect
    public override float FireRate { get { return .2f; } }
    public override float SlowdownFactor { get { return .8f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +11.7f, -11.7f, +10.5f, -10.5f, +9.3f, -9.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return .3f; } }
    public override float IntervalTime { get { return .1f; } }
    public override float ForceOpposite { get { return 12f; } }
    public override float FlashLightFactor { get { return 6f; } }
}
public class DualBerretas : Gun {
    public override float BulletDamage { get { return 7f; } }
    public override float BulletCriticalDamage { get { return 15f; } }
    public override GunType GunType { get { return GunType.DualBerretas; } }
    public override int NumberOfRay { get { return 2; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -0,+0,
            };
        }
    }
    // Basic DualBerretas Light
    public override Color GunLightColor { get { return Color.red; } }
    public override float PointLightIntensity { get { return 1.5f; } }
    public override float PointLightRange { get { return 9f; } }
    public override float LrStarWidth { get { return .05f; } }
    public override float LrEndWidth { get { return .05f; } }
    // Laser DualBerretas Effect
    public override float ShootRange { get { return 7f; } }
    // Shoot DualBerretas Effect
    public override float FireRate { get { return .7f; } }
    public override float SlowdownFactor { get { return 1f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +14.7f, -14.7f, +13.5f, -13.5f, +12.3f, -12.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return .5f; } }
    public override float IntervalTime { get { return .2f; } }
    public override float ForceOpposite { get { return 12f; } }
    public override float FlashLightFactor { get { return 7.5f; } }
}
public class Knife : Gun {
    public override float BulletDamage { get { return 4f; } }
    public override float BulletCriticalDamage { get { return 7f; } }
    public override GunType GunType { get { return GunType.Knife; } }
    public override int NumberOfRay { get { return 1; } }
    public override float[] RightLeftRotationArray {
        get {
            return new float[] {
                0, -0,+0,
            };
        }
    }
    // Basic Knife Light
    public override Color GunLightColor { get { return Color.gray; } }
    public override float PointLightIntensity { get { return 1f; } }
    public override float PointLightRange { get { return 6.5f; } }
    public override float LrStarWidth { get { return .05f; } }
    public override float LrEndWidth { get { return .05f; } }
    // Laser Knife Effect
    public override float ShootRange { get { return .4f; } }
    // Shoot Knife Effect
    public override float FireRate { get { return .5f; } }
    public override float SlowdownFactor { get { return 1.3f; } }
    public override float[] FloatSpinArray {
        get {
            float[] floatArray = new float[6] { +5.7f, -5.7f, +4.5f, -4.5f, +3.3f, -3.3f }; ;
            return floatArray;
        }
    }
    public override float ShakeIntensity { get { return .01f; } }
    public override float IntervalTime { get { return .01f; } }
    public override float ForceOpposite { get { return 1f; } }
    public override float FlashLightFactor { get { return 3f; } }
}