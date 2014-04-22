// <copyright file="IInputProvider.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

public interface IInputProvider
{
    bool IsActive
    {
        get;
    }

    bool IsUp
    {
        get;
    }

    bool IsDown
    {
        get;
    }
}
