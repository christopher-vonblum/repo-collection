using System;

namespace Client.cs
{
    public static class Interceptor
    {
        public static void Enter()
        {
            Console.WriteLine("Enter");
        }

        public static void Exit()
        {
            Console.WriteLine("Exit");
        }
    }
}