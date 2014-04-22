// <copyright file="IProfileManagement.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

public interface IProfileManagement
{
    void SetId(int id);

    void SetPassword(string password);

    void SetName(string name);

    void ClearUnsynchronizedScores();
}