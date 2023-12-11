using System.Numerics;

namespace AoC.Common.Maps;

public record struct Rectangle<T>(T X, T Y, T Width, T Height) where T : INumber<T>;