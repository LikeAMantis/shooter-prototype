// using System;
//
// namespace Movement.Crouch
// {
//     public enum CrouchableTypes {BakedCrouch, ProceduralCrouch}
//     
//     public class CrouchableFactory
//     {
//         public static ICrouchable GetCrouchable(CrouchableTypes crouchableType)
//         {
//             return crouchableType switch
//             {
//                 CrouchableTypes.BakedCrouch => new BakedCrouch(),
//                 CrouchableTypes.ProceduralCrouch => new ProceduralCrouch(),
//                 _ => throw new ArgumentOutOfRangeException(nameof(crouchableType), crouchableType, null)
//             };
//         }
//     }
// }