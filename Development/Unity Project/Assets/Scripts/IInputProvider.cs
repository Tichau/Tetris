// <copyright file="IInputProvider.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

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
