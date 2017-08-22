using System;

namespace StockExchangeGame.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    // ReSharper disable once UnusedMember.Global
    public class AutoIncrementAttribute : Attribute
    {
    }
}