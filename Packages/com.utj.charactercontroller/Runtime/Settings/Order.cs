
namespace Unity.TinyCharacterController.Utility
{
    public static class Order
    {
        public const int GameObjectPool = -1000;
        public const int IndicatorRegister = -100;
        
        // ----------------------------------
        // early update
        // ----------------------------------

        // Prepare Process
        public const int PrepareEarlyUpdate = -1000;
        
        // ----------------------------------
        // update
        // ----------------------------------

        public const int Check = -200;
        public const int Gravity = -100;
        // Self-contained
        public const int Effect = -50;
        // Control
        public const int Control = 5;
        
        // ----------------------------------
        // Apply calculation results (ExecuteOrder)
        // ----------------------------------

        public const int EarlyUpdateBrain = -101;   // Before InputSystem
        public const int UpdateBrain = 10;          // Brain update timing
        public const int PostUpdate = 100;          // After Brain
        public const int UpdateIK = 50;             // IK update timing
    }
}
