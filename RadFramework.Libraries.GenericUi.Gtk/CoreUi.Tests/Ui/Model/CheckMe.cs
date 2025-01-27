using System;

namespace CoreUi.Tests.Ui.Model
{
    [Flags]
    public enum CheckMe
    {
        A = 1,
        B = 1 << 1,
        C = 1 << 2,
        D = 1 << 3,
        E = 1 << 4,
        F = 1 << 5
    }
}