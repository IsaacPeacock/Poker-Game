using System;
using Microsoft.Xna.Framework;
namespace Violet;

public interface IClickable
{
	Rectangle rectangleArea { get; }

	void Clicked();
}