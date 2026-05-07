using UnityEngine;
public class LookRegion : MonoBehaviour
{
    public enum dir
    {
        Left,
        Right
    }
    public enum strength
    {
        Min,
        Med,
        Max
    }
    public dir direction;
    public strength lookStrength;
}


//[CreateAssetMenu(fileName = "NewLookRegion", menuName = "Look Region")]
//public class LookRegion : ScriptableObject
//{

//    public enum dir
//    {
//        Left,
//        Right
//    }
//    public enum strength
//    {
//        Min,
//        Med,
//        Max
//    }
//    public dir direction;
//    public strength lookStrength;
//}