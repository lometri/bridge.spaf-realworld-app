﻿namespace realworld.spaf.Services
{
    public interface ISettings
    {
        string ApiUri { get; }
        int ArticleInPage { get; }
    }
}